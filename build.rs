fn main() {
    if cfg!(target_os = "windows") {
        let mut res = winresource::WindowsResource::new();

        if std::path::Path::new("resources/icon.ico").exists() {
            res.set_icon("resources/icon.ico");
        }

        res.set("ProductName", "FlashPatch");
        res.set("FileDescription", "FlashPatch");
        res.set("CompanyName", "Clean Flash Team");
        res.set("LegalCopyright", "Clean Flash Team");

        res.set_manifest(r#"<?xml version="1.0" encoding="utf-8"?>
<assembly manifestVersion="1.0" xmlns="urn:schemas-microsoft-com:asm.v1">
    <assemblyIdentity version="1.0.0.0" name="FlashPatch.app"/>
    <trustInfo xmlns="urn:schemas-microsoft-com:asm.v2">
        <security>
            <requestedPrivileges xmlns="urn:schemas-microsoft-com:asm.v3">
                <requestedExecutionLevel level="requireAdministrator" uiAccess="false" />
            </requestedPrivileges>
        </security>
    </trustInfo>
    <compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
        <application>
            <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}"/>
            <supportedOS Id="{1f676c76-80e1-4239-95bb-83d0f6d0da78}"/>
            <supportedOS Id="{4a2f28e3-53b9-4441-ba9c-d69d4a4a6e38}"/>
            <supportedOS Id="{35138b9a-5d96-4fbd-8e2d-a2440225f93a}"/>
            <supportedOS Id="{e2011457-1546-43c5-a5fe-008deee3d3f0}"/>
        </application>
    </compatibility>
    <application xmlns="urn:schemas-microsoft-com:asm.v3">
        <windowsSettings>
            <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">True/PM</dpiAware>
            <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">PerMonitorV2</dpiAwareness>
        </windowsSettings>
    </application>
</assembly>
"#);

        let _ = res.compile();
    }
}
