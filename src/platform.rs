// Platform-specific code for finding and preparing Flash Player binaries.

use std::path::PathBuf;

/// Information about a discovered Flash Player binary on the system.
#[derive(Debug, Clone)]
pub struct FlashBinary {
    pub path: PathBuf,
    pub name: String,
}

/// Find Flash Player binaries on the system (Windows only).
/// On Linux/Mac, returns an empty vec — users must use "Patch File..." instead.
#[cfg(target_os = "windows")]
pub fn find_flash_binaries() -> Vec<FlashBinary> {
    let mut binaries = Vec::new();

    let windows_dir = std::env::var("WINDIR").unwrap_or_else(|_| "C:\\Windows".to_string());
    let is_64bit = cfg!(target_pointer_width = "64") || std::env::var("PROCESSOR_ARCHITEW6432").is_ok();

    // Flash 32-bit directory
    let flash_dir_32 = if is_64bit {
        PathBuf::from(&windows_dir).join("SysWOW64").join("Macromed").join("Flash")
    } else {
        PathBuf::from(&windows_dir).join("System32").join("Macromed").join("Flash")
    };

    // Flash 64-bit directory
    let flash_dir_64 = if is_64bit {
        Some(PathBuf::from(&windows_dir).join("System32").join("Macromed").join("Flash"))
    } else {
        None
    };

    // Scan both directories for DLLs and OCX files
    let scan_patterns = [
        ("NPSWF*.dll", "Flash Player NPAPI Plugin"),
        ("Flash*.ocx", "Flash Player ActiveX Control"),
        ("pepflashplayer*.dll", "Flash Player Pepper Plugin"),
    ];

    for dir in [Some(&flash_dir_32), flash_dir_64.as_ref()].into_iter().flatten() {
        if !dir.exists() {
            continue;
        }
        if let Ok(entries) = std::fs::read_dir(dir) {
            for entry in entries.flatten() {
                let path = entry.path();
                let fname = path.file_name().unwrap_or_default().to_string_lossy().to_string();
                let fname_lower = fname.to_lowercase();

                for (pattern_prefix, desc) in &scan_patterns {
                    // Simple glob matching: check prefix/suffix
                    let prefix = pattern_prefix.split('*').next().unwrap_or("").to_lowercase();
                    let suffix = pattern_prefix.split('*').last().unwrap_or("").to_lowercase();

                    if fname_lower.starts_with(&prefix) && fname_lower.ends_with(&suffix) {
                        binaries.push(FlashBinary {
                            path: path.clone(),
                            name: format!("{} ({})", desc, fname),
                        });
                    }
                }
            }
        }
    }

    binaries
}

#[cfg(not(target_os = "windows"))]
pub fn find_flash_binaries() -> Vec<FlashBinary> {
    Vec::new()
}

/// Disable Flash telemetry services (Windows only).
/// Stops and disables FlashCenterService and Flash Helper Service,
/// and prevents FlashCenterService.exe and FlashHelperService.exe from executing.
#[cfg(target_os = "windows")]
pub fn disable_telemetry(log: &mut Vec<String>) {
    use std::ffi::OsStr;
    use std::os::windows::ffi::OsStrExt;

    let telemetry_services = ["FlashCenterService", "Flash Helper Service"];
    let telemetry_apps = ["FlashCenterService.exe", "FlashHelperService.exe"];

    // Stop and disable services
    for service_name in &telemetry_services {
        match stop_and_disable_service(service_name) {
            Ok(()) => log.push(format!("Disabled service: {}", service_name)),
            Err(e) => log.push(format!("Could not disable service {}: {}", service_name, e)),
        }
    }

    // Prevent telemetry apps from running via Image File Execution Options
    let ifeo_base = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options";
    for app in &telemetry_apps {
        let key_path = format!("{}\\{}", ifeo_base, app);
        match set_registry_debugger(&key_path) {
            Ok(()) => log.push(format!("Blocked execution of: {}", app)),
            Err(e) => log.push(format!("Could not block {}: {}", app, e)),
        }
    }
}

#[cfg(not(target_os = "windows"))]
pub fn disable_telemetry(log: &mut Vec<String>) {
    log.push("Telemetry disable: not applicable on this platform.".to_string());
}

/// Take ownership of a file and grant full control to the current user (Windows only).
#[cfg(target_os = "windows")]
pub fn take_ownership(path: &std::path::Path, log: &mut Vec<String>) {
    // Remove read-only attribute
    if let Ok(metadata) = std::fs::metadata(path) {
        let mut perms = metadata.permissions();
        #[allow(clippy::permissions_set_readonly_false)]
        perms.set_readonly(false);
        let _ = std::fs::set_permissions(path, perms);
    }

    // Enable privileges
    let _ = enable_privilege("SeRestorePrivilege");
    let _ = enable_privilege("SeTakeOwnershipPrivilege");

    match take_ownership_impl(path) {
        Ok(()) => log.push(format!("Took ownership of: {}", path.display())),
        Err(e) => log.push(format!("Could not take ownership of {}: {}", path.display(), e)),
    }
}

#[cfg(not(target_os = "windows"))]
pub fn take_ownership(_path: &std::path::Path, _log: &mut Vec<String>) {
    // No-op on non-Windows.
}

/// Create a backup of a file (copies to <original>.bak alongside the file).
pub fn backup_file(path: &std::path::Path) -> std::io::Result<PathBuf> {
    let backup_path = path.with_extension(
        format!("{}.bak",
            path.extension().map(|e| e.to_string_lossy().to_string()).unwrap_or_default()
        )
    );
    std::fs::copy(path, &backup_path)?;
    Ok(backup_path)
}

// ---------------------------------------------------------------------------
// Windows-specific helpers
// ---------------------------------------------------------------------------

#[cfg(target_os = "windows")]
fn stop_and_disable_service(name: &str) -> Result<(), String> {
    use std::ffi::OsStr;
    use std::os::windows::ffi::OsStrExt;
    use winapi::um::winsvc::*;
    use winapi::um::winnt::*;

    unsafe {
        let sc_manager = OpenSCManagerW(
            std::ptr::null(),
            std::ptr::null(),
            SC_MANAGER_ALL_ACCESS,
        );
        if sc_manager.is_null() {
            return Err("Cannot open SCManager".to_string());
        }

        let wide_name: Vec<u16> = OsStr::new(name).encode_wide().chain(std::iter::once(0)).collect();
        let service = OpenServiceW(sc_manager, wide_name.as_ptr(), SERVICE_ALL_ACCESS);
        if service.is_null() {
            CloseServiceHandle(sc_manager);
            return Err(format!("Service '{}' not found", name));
        }

        // Try to stop the service
        let mut status: SERVICE_STATUS = std::mem::zeroed();
        let _ = ControlService(service, SERVICE_CONTROL_STOP, &mut status);

        // Change start type to disabled
        let result = ChangeServiceConfigW(
            service,
            SERVICE_NO_CHANGE,
            SERVICE_DISABLED,
            SERVICE_NO_CHANGE,
            std::ptr::null(),
            std::ptr::null(),
            std::ptr::null_mut(),
            std::ptr::null(),
            std::ptr::null(),
            std::ptr::null(),
            std::ptr::null(),
        );

        CloseServiceHandle(service);
        CloseServiceHandle(sc_manager);

        if result == 0 {
            Err(format!("Failed to disable service '{}'", name))
        } else {
            Ok(())
        }
    }
}

#[cfg(target_os = "windows")]
fn set_registry_debugger(key_path: &str) -> Result<(), String> {
    use std::ffi::OsStr;
    use std::os::windows::ffi::OsStrExt;
    use windows_sys::Win32::System::Registry::*;

    let wide_path: Vec<u16> = OsStr::new(key_path).encode_wide().chain(std::iter::once(0)).collect();
    let debugger_value = "invalid_debugger.exe";
    let wide_value: Vec<u16> = OsStr::new("Debugger").encode_wide().chain(std::iter::once(0)).collect();
    let wide_data: Vec<u16> = OsStr::new(debugger_value).encode_wide().chain(std::iter::once(0)).collect();

    unsafe {
        let mut hkey: HKEY = std::ptr::null_mut();
        let mut disposition: u32 = 0;
        let result = RegCreateKeyExW(
            HKEY_LOCAL_MACHINE,
            wide_path.as_ptr(),
            0,
            std::ptr::null(),
            0,
            KEY_WRITE,
            std::ptr::null(),
            &mut hkey,
            &mut disposition,
        );
        if result != 0 {
            return Err(format!("Cannot create registry key (error {})", result));
        }

        let result = RegSetValueExW(
            hkey,
            wide_value.as_ptr(),
            0,
            REG_SZ,
            wide_data.as_ptr() as *const u8,
            (wide_data.len() * 2) as u32,
        );
        RegCloseKey(hkey);

        if result != 0 {
            Err(format!("Cannot set registry value (error {})", result))
        } else {
            Ok(())
        }
    }
}

#[cfg(target_os = "windows")]
fn enable_privilege(name: &str) -> Result<(), String> {
    use std::ffi::OsStr;
    use std::os::windows::ffi::OsStrExt;
    use winapi::um::processthreadsapi::*;
    use winapi::um::securitybaseapi::*;
    use winapi::um::winbase::*;
    use winapi::um::winnt::*;
    use winapi::um::handleapi::*;

    unsafe {
        let mut token: winapi::um::winnt::HANDLE = std::ptr::null_mut();
        if OpenProcessToken(
            winapi::um::processthreadsapi::GetCurrentProcess(),
            TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,
            &mut token,
        ) == 0
        {
            return Err("Cannot open process token".to_string());
        }

        let wide_name: Vec<u16> = OsStr::new(name).encode_wide().chain(std::iter::once(0)).collect();
        let mut luid: LUID = std::mem::zeroed();
        if LookupPrivilegeValueW(std::ptr::null(), wide_name.as_ptr(), &mut luid) == 0 {
            CloseHandle(token);
            return Err(format!("Cannot lookup privilege '{}'", name));
        }

        let mut tp: TOKEN_PRIVILEGES = std::mem::zeroed();
        tp.PrivilegeCount = 1;
        tp.Privileges[0].Luid = luid;
        tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

        let result = AdjustTokenPrivileges(
            token,
            0,
            &mut tp,
            std::mem::size_of::<TOKEN_PRIVILEGES>() as u32,
            std::ptr::null_mut(),
            std::ptr::null_mut(),
        );
        CloseHandle(token);

        if result == 0 {
            Err(format!("Cannot adjust privileges for '{}'", name))
        } else {
            Ok(())
        }
    }
}

#[cfg(target_os = "windows")]
fn take_ownership_impl(path: &std::path::Path) -> Result<(), String> {
    use std::ffi::OsStr;
    use std::os::windows::ffi::OsStrExt;
    use windows_sys::Win32::Security::Authorization::*;
    use windows_sys::Win32::Security::*;
    use windows_sys::Win32::Foundation::*;

    let wide_path: Vec<u16> = OsStr::new(path).encode_wide().chain(std::iter::once(0)).collect();

    unsafe {
        // Get current user SID
        let mut token: windows_sys::Win32::Foundation::HANDLE = std::mem::zeroed();
        if windows_sys::Win32::System::Threading::OpenProcessToken(
            windows_sys::Win32::System::Threading::GetCurrentProcess(),
            TOKEN_QUERY,
            &mut token,
        ) == 0
        {
            return Err("Cannot open process token".to_string());
        }

        let mut token_user_size: u32 = 0;
        let _ = GetTokenInformation(token, TokenUser, std::ptr::null_mut(), 0, &mut token_user_size);
        let mut buffer = vec![0u8; token_user_size as usize];
        if GetTokenInformation(
            token,
            TokenUser,
            buffer.as_mut_ptr() as *mut _,
            token_user_size,
            &mut token_user_size,
        ) == 0
        {
            windows_sys::Win32::Foundation::CloseHandle(token);
            return Err("Cannot get token information".to_string());
        }
        windows_sys::Win32::Foundation::CloseHandle(token);

        let token_user = &*(buffer.as_ptr() as *const TOKEN_USER);
        let sid = token_user.User.Sid;

        // Set owner
        let result = SetNamedSecurityInfoW(
            wide_path.as_ptr() as *mut _,
            SE_FILE_OBJECT,
            OWNER_SECURITY_INFORMATION,
            sid,
            std::ptr::null_mut(),
            std::ptr::null_mut(),
            std::ptr::null_mut(),
        );
        if result != 0 {
            return Err(format!("SetNamedSecurityInfoW (owner) failed: {}", result));
        }

        // Grant full control
        let mut ea: EXPLICIT_ACCESS_W = std::mem::zeroed();
        ea.grfAccessPermissions = 0x1F01FF; // FILE_ALL_ACCESS
        ea.grfAccessMode = SET_ACCESS;
        ea.grfInheritance = NO_INHERITANCE;
        ea.Trustee.TrusteeForm = TRUSTEE_IS_SID;
        ea.Trustee.ptstrName = sid as *mut u16;

        let mut new_dacl: *mut ACL = std::ptr::null_mut();
        let result = SetEntriesInAclW(1, &mut ea, std::ptr::null_mut(), &mut new_dacl);
        if result != 0 {
            return Err(format!("SetEntriesInAclW failed: {}", result));
        }

        let result = SetNamedSecurityInfoW(
            wide_path.as_ptr() as *mut _,
            SE_FILE_OBJECT,
            DACL_SECURITY_INFORMATION,
            std::ptr::null_mut(),
            std::ptr::null_mut(),
            new_dacl,
            std::ptr::null_mut(),
        );

        if !new_dacl.is_null() {
            windows_sys::Win32::Foundation::LocalFree(new_dacl as *mut _);
        }

        if result != 0 {
            Err(format!("SetNamedSecurityInfoW (DACL) failed: {}", result))
        } else {
            Ok(())
        }
    }
}

/// Disable WoW64 filesystem redirection (Windows only).
#[cfg(target_os = "windows")]
pub fn disable_wow64_redirection() -> *mut std::ffi::c_void {
    use std::ptr;
    extern "system" {
        fn Wow64DisableWow64FsRedirection(old_value: *mut *mut std::ffi::c_void) -> i32;
    }
    let mut old_value: *mut std::ffi::c_void = ptr::null_mut();
    unsafe {
        Wow64DisableWow64FsRedirection(&mut old_value);
    }
    old_value
}

#[cfg(target_os = "windows")]
pub fn enable_wow64_redirection(old_value: *mut std::ffi::c_void) {
    extern "system" {
        fn Wow64RevertWow64FsRedirection(old_value: *mut std::ffi::c_void) -> i32;
    }
    unsafe {
        Wow64RevertWow64FsRedirection(old_value);
    }
}

#[cfg(not(target_os = "windows"))]
pub fn disable_wow64_redirection() -> *mut std::ffi::c_void {
    std::ptr::null_mut()
}

#[cfg(not(target_os = "windows"))]
pub fn enable_wow64_redirection(_old_value: *mut std::ffi::c_void) {}
