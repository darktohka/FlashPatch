using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Management;
using System.ServiceProcess;

namespace FlashPatch {
    public class Patcher {

        private static string executionOptionsPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\";
        private static List<string> telemetryApps = new List<string>() { "FlashCenterService.exe", "FlashHelperService.exe" };
        private static List<string> telemetryServices = new List<string>() { "FlashCenterService", "Flash Helper Service" };

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        static Patcher() {
            WinAPI.ModifyPrivilege(PrivilegeName.SeRestorePrivilege, true);
            WinAPI.ModifyPrivilege(PrivilegeName.SeTakeOwnershipPrivilege, true);
        }

        private static string GetWindowsDir() {
            return Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        }

        private static string GetFlashDir32() {
            if (Environment.Is64BitOperatingSystem) {
                return Path.Combine(GetWindowsDir(), "SysWOW64", "Macromed", "Flash");
            } else {
                return Path.Combine(GetWindowsDir(), "System32", "Macromed", "Flash");
            }
        }

        private static string GetFlashDir64() {
            if (Environment.Is64BitOperatingSystem) {
                return Path.Combine(GetWindowsDir(), "System32", "Macromed", "Flash");
            } else {
                return null;
            }
        }

        private static string GetVersion(string filename) {
            return FileVersionInfo.GetVersionInfo(filename).FileVersion;
        }

        private static bool IsSharingViolation(Exception e) {
            return ((uint)e.HResult) == 0x80070020;
        }

        private static void ShowError(string message) {
            MessageBox.Show(message, "FlashPatch!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void AppendItems(StringBuilder builder, string message, List<string> items) {
            if (items.Count <= 0) {
                return;
            }

            builder.AppendLine(message);

            foreach (string item in items) {
                builder.AppendLine(item);
            }

            builder.AppendLine();
        }

        private static void StopService(string serviceName) {
            ServiceController service = new ServiceController(serviceName);
            
            service.Stop();
        }

        private static void ChangeServiceStartMode(string serviceName, string startMode) {
            using (ManagementObject obj = new ManagementObject(string.Format("Win32_Service.Name=\"{0}\"", serviceName))) {
                obj.InvokeMethod("ChangeStartMode", new object[] { startMode });
            }
        }

        private static void PreventExecution(string filename) {
            RegistryKey key = Registry.LocalMachine.CreateSubKey(executionOptionsPath + filename);

            key.SetValue("Debugger", Guid.NewGuid().ToString() + ".exe");
        }

        private static void RestoreExecution(string filename) {
            Registry.LocalMachine.DeleteSubKeyTree(executionOptionsPath + filename);
        }

        private static void TakeOwnership(string filename) {
            // Remove read-only attribute
            File.SetAttributes(filename, File.GetAttributes(filename) & ~FileAttributes.ReadOnly);

            FileSecurity security = new FileSecurity();

            SecurityIdentifier sid = WindowsIdentity.GetCurrent().User;
            security.SetOwner(sid);
            security.SetAccessRule(new FileSystemAccessRule(sid, FileSystemRights.FullControl, AccessControlType.Allow));

            File.SetAccessControl(filename, security);
        }

        public static void PatchAll() {
            if (MessageBox.Show("Are you sure you want to patch your system-wide Flash plugins to allow Flash games to be played in your browser?", "FlashPatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            if (MessageBox.Show("Have you closed ALL your browser windows yet?\n\nIf not, please close them right now!", "FlashPatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            if (MessageBox.Show("WARNING!\n\nThe developers of this program do not assume any responsibility for the usage of this tool.\n\nEven though the developers have tried their best to ensure the quality of this tool, it may introduce instability, or even crash your computer.\n\nAll changes made by the program may be reverted using the \"Restore\" button, but even this option is provided on a best-effort basis.\n\nAll responsibility falls upon your shoulders.\n\nEven so, are you sure you want to continue?", "FlashPatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
                return;
            }

            IntPtr redirection = DisableRedirection();

            string flashDir32 = GetFlashDir32();

            if (!Directory.Exists(flashDir32)) {
                ShowError(string.Format("Could not find 32-bit Flash directory!\n\n{0} does not exist.", flashDir32));
                return;
            }

            bool x64 = Environment.Is64BitOperatingSystem;
            string flashDir64 = null;

            if (x64) {
                flashDir64 = GetFlashDir64();

                if (!Directory.Exists(flashDir64)) {
                    ShowError(string.Format("Could not find 64-bit Flash directory!\n\n{0} does not exist.", flashDir64));
                    return;
                }
            }

            foreach (string service in telemetryServices) {
                try {
                    ChangeServiceStartMode(service, "Disabled");
                } catch (Exception e) {
                    // It's fine if this doesn't work
                }

                try {
                    StopService(service);
                } catch (Exception e) {
                    // It's fine if this doesn't work
                }
            }

            try {
                foreach (string telemetryApp in telemetryApps) {
                    PreventExecution(telemetryApp);
                }
            } catch (Exception e) {
                ShowError(string.Format("Could not disable Flash Player telemetry! Are you running this application as administrator?\n\n{0}", e.Message));
            }

            List<string> patched = new List<string>();
            List<string> alreadyPatched = new List<string>();
            List<string> notFound = new List<string>();
            List<string> incompatibleVersion = new List<string>();
            List<string> ownershipFailed = new List<string>();
            List<string> locked = new List<string>();
            List<string> errors = new List<string>();

            string backupFolder = Path.Combine(Environment.CurrentDirectory, "Backup");
            bool madeBackupFolder = false;

            foreach (PatchableBinary binary in Patches.GetBinaries()) {
                if (!binary.HasFilenames()) {
                    // This is a special binary, as we do not specifically look for it.
                    // This binary can only be patched using the "Patch File..." option
                    continue;
                }

                bool binaryX64 = binary.IsX64();

                if (binaryX64 && !x64) {
                    // This is a 64-bit binary, but we are not on a 64-bit system.
                    continue;
                }

                string name = binary.GetName();
                bool found = false;

                foreach (string filename in binary.GetFilenames()) {
                    List<string> paths = new List<string>();

                    paths.Add(Path.Combine(binaryX64 ? flashDir64 : flashDir32, filename));
                    paths.AddRange(binary.GetAlternatePaths());

                    foreach (string path in paths) {
                        // Remove our current binary from the not found list first.
                        if (notFound.Contains(name)) {
                            notFound.Remove(name);
                        }

                        if (!File.Exists(path)) {
                            continue;
                        }

                        string version = GetVersion(path);

                        if (!binary.GetVersion().Equals(version)) {
                            // We've encountered an incompatible version.
                            found = true;
                            incompatibleVersion.Add(string.Format("{0} ({1})", name, version));
                            continue;
                        }

                        long size = new FileInfo(path).Length;

                        if (binary.HasFileSize() && binary.GetFileSize() != size) {
                            // This file's size does not match the expected file size.
                            continue;
                        }

                        found = true;

                        try {
                            TakeOwnership(path);
                        } catch {
                            // We failed to get ownership of the file...
                            // No continue here, we still want to try to patch the file
                            ownershipFailed.Add(name);
                        }

                        try {
                            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read)) {
                                if (!binary.IsPatchable(fileStream)) {
                                    // This binary has already been patched.
                                    alreadyPatched.Add(name);
                                    continue;
                                }
                            }

                            if (!madeBackupFolder && !Directory.Exists(backupFolder)) {
                                Directory.CreateDirectory(backupFolder);
                                madeBackupFolder = true;
                            }

                            // Back up the current plugin to our backup folder
                            File.Copy(path, Path.Combine(backupFolder, binary.GetBackupFileName(filename)), true);

                            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite)) {
                                // Apply all pending binary patches!
                                binary.PatchFile(fileStream);
                            }

                            if (!patched.Contains(name)) {
                                patched.Add(name);
                            }
                        } catch (Exception e) {
                            if (IsSharingViolation(e)) {
                                // This is a sharing violation; i.e. the file is currently being used.
                                locked.Add(name);
                            } else {
                                errors.Add(e.Message);
                            }
                        }
                    }
                }

                if (!found) {
                    // Add this binary to the not found list.
                    if (!patched.Contains(name) && !notFound.Contains(name)) {
                        notFound.Add(name);
                    }
                }
            }

            EnableRedirection(redirection);

            StringBuilder report = new StringBuilder();
            MessageBoxIcon icon = MessageBoxIcon.Information;

            AppendItems(report, "Successfully patched these plugins:", patched);
            AppendItems(report, "These plugins have already been patched:", alreadyPatched);
            AppendItems(report, "These plugins have not been found on your system:", notFound);
            AppendItems(report, "These plugins are incompatible with the patch because their version is outdated:", incompatibleVersion);
            AppendItems(report, "These plugins could not be patched because their respective browser is currently open:", locked);
            AppendItems(report, "Caught exceptions:", errors);

            if (incompatibleVersion.Count > 0 || locked.Count > 0 || errors.Count > 0) {
                icon = MessageBoxIcon.Warning;
                report.AppendLine("Errors have been encountered during the patching process.\nPlease try again after reading the message above carefully.\nIf the browser you're using has been patched successfully, then no more action is required.");
            } else if (patched.Count > 0) {
                report.AppendLine("CONGRATULATIONS! The patching process has completed as expected. Enjoy your Flash games!");
            } else if (alreadyPatched.Count > 0) {
                report.AppendLine("Flash Player has already been patched on this system.\n\nNo more action is required! Enjoy your games!");
            } else {
                report.AppendLine("No action has been taken.");
            }

            MessageBox.Show(report.ToString(), "FlashPatch!", MessageBoxButtons.OK, icon);
        }

        public static void PatchFiles(string[] paths) {
            List<string> patched = new List<string>();
            List<string> alreadyPatched = new List<string>();
            List<string> notPatched = new List<string>();
            List<string> ownershipFailed = new List<string>();
            List<string> locked = new List<string>();
            List<string> errors = new List<string>();

            foreach (string path in paths) {
                string filename = Path.GetFileName(path);
                long size = new FileInfo(path).Length;

                try {
                    TakeOwnership(path);
                } catch {
                    // We failed to get ownership of the file...
                    // No continue here, we still want to try to patch the file
                    ownershipFailed.Add(filename);
                }

                bool complete = false;

                foreach (PatchableBinary binary in Patches.GetBinaries()) {
                    if (binary.HasFileSize() && binary.GetFileSize() != size) {
                        continue;
                    }

                    try {
                        using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read)) {
                            if (!binary.IsPatchable(fileStream)) {
                                if (binary.IsPatched(fileStream)) {
                                    // This binary has already been patched.
                                    alreadyPatched.Add(filename);
                                    complete = true;
                                    break;
                                }

                                // This binary hasn't been patched, but it's not patchable either.
                                // This means that the current patch does not match this file.
                                continue;
                            }
                        }

                        // Back up the plugin to the same folder with a ".bak" extension
                        File.Copy(path, Path.Combine(Path.GetDirectoryName(path), filename + ".bak"), true);

                        using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite)) {
                            // Apply all pending binary patches!
                            binary.PatchFile(fileStream);
                        }

                        patched.Add(string.Format("{0} (patch used: {1})", filename, binary.GetName()));
                        complete = true;
                        break;
                    } catch (Exception e) {
                        if (IsSharingViolation(e)) {
                            // This is a sharing violation; i.e. the file is currently being used.
                            locked.Add(filename);
                        } else {
                            errors.Add(e.Message);
                        }
                    }
                }

                if (!complete) {
                    notPatched.Add(filename);
                }
            }

            StringBuilder report = new StringBuilder();
            MessageBoxIcon icon = MessageBoxIcon.Information;

            AppendItems(report, "Successfully patched these binaries:", patched);
            AppendItems(report, "These binaries have already been patched:", alreadyPatched);
            AppendItems(report, "These binaries have not been patched, as compatible patches do not exist:", notPatched);
            AppendItems(report, "These binaries could not be patched because their respective browser is currently open:", locked);
            AppendItems(report, "Failed to take ownership of the following binaries:", ownershipFailed);
            AppendItems(report, "Caught exceptions:", errors);

            if (locked.Count > 0 || errors.Count > 0) {
                icon = MessageBoxIcon.Warning;
                report.AppendLine("Errors have been encountered during the patching process.\nPlease try again after reading the message above carefully.");
            } else if (notPatched.Count > 0) {
                icon = MessageBoxIcon.Warning;
                report.AppendLine("Sorry, but we couldn't patch some of your binaries.");
            } else if (patched.Count > 0) {
                report.AppendLine("Great work! All binaries have been successfully patched.");
            } else if (alreadyPatched.Count > 0) {
                report.AppendLine("Looks like all of your binaries have already been patched!");
            } else {
                report.AppendLine("No action has been taken.");
            }

            MessageBox.Show(report.ToString(), "FlashPatch!", MessageBoxButtons.OK, icon);
        }

        public static IntPtr DisableRedirection() {
            IntPtr wow64Value = IntPtr.Zero;

            // Disable file system indirection (otherwise we can't read System32)
            Wow64DisableWow64FsRedirection(ref wow64Value);
            return wow64Value;
        }

        public static void EnableRedirection(IntPtr wow64Value) {
            // Enable file system indirection.
            Wow64RevertWow64FsRedirection(wow64Value);
        }

        public static void RestoreAll() {
            if (MessageBox.Show("Are you sure you want to restore your Flash Plugin backups?", "FlashPatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            foreach (string service in telemetryServices) {
                try {
                    ChangeServiceStartMode(service, "Automatic");
                } catch (Exception e) {
                    // It's fine if this doesn't work
                }
            }

            foreach (string telemetryApp in telemetryApps) {
                try {
                    RestoreExecution(telemetryApp);
                } catch (Exception e) {
                    // It's fine if this doesn't work.
                    // This means that the redirection has been removed.
                }
            }

            string backupFolder = Path.Combine(Environment.CurrentDirectory, "Backup");

            if (!Directory.Exists(backupFolder)) {
                ShowError("No backups are currently available.");
                return;
            }

            IntPtr redirection = DisableRedirection();

            string flashDir32 = GetFlashDir32();

            if (!Directory.Exists(flashDir32)) {
                ShowError(string.Format("Could not find 32-bit Flash directory!\n\n{0} does not exist.", flashDir32));
                return;
            }

            bool x64 = Environment.Is64BitOperatingSystem;
            string flashDir64 = null;

            if (x64) {
                flashDir64 = GetFlashDir64();

                if (!Directory.Exists(flashDir64)) {
                    ShowError(string.Format("Could not find 64-bit Flash directory!\n\n{0} does not exist.", flashDir64));
                    return;
                }
            }

            List<string> restored = new List<string>();
            List<string> locked = new List<string>();
            List<string> errors = new List<string>();

            foreach (PatchableBinary binary in Patches.GetBinaries()) {
                if (!binary.HasFilenames()) {
                    continue;
                }

                bool binaryX64 = binary.IsX64();

                if (binaryX64 && !x64) {
                    // This is a 64-bit binary, but we are not on a 64-bit system.
                    continue;
                }

                foreach (string filename in binary.GetFilenames()) {
                    string backupPath = Path.Combine(backupFolder, binary.GetBackupFileName(filename));

                    if (!File.Exists(backupPath)) {
                        continue;
                    }

                    string name = binary.GetName();
                    string path = Path.Combine(binaryX64 ? flashDir64 : flashDir32, filename);

                    try {
                        File.Copy(backupPath, path, true);
                        restored.Add(name);
                    } catch (Exception e) {
                        if (IsSharingViolation(e)) {
                            // This is a sharing violation; i.e. the file is currently being used.
                            locked.Add(name);
                        } else {
                            errors.Add(e.Message);
                        }
                    }
                }
            }

            EnableRedirection(redirection);

            StringBuilder report = new StringBuilder();
            MessageBoxIcon icon = MessageBoxIcon.Information;

            AppendItems(report, "Successfully restored these plugins to their original, unpatched version:", restored);
            AppendItems(report, "These plugins could not be restored because their respective browser is currently open:", locked);
            AppendItems(report, "Caught exceptions:", errors);

            if (locked.Count > 0 || errors.Count > 0) {
                icon = MessageBoxIcon.Warning;
                report.AppendLine("Errors have been encountered during the restoration process. Please try again after reading the message above carefully.");
            } else if (restored.Count > 0) {
                report.AppendLine("All plugins have been restored from the previous backup!\nNo more action is necessary.");
            } else {
                ShowError("No backups are currently available.");
                return;
            }

            MessageBox.Show(report.ToString(), "FlashPatch!", MessageBoxButtons.OK, icon);
        }
    }
}
