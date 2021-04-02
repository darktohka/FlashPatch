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

namespace FlashPatch {
    public class Patcher {
        // All of these patches are for the latest Adobe Flash
        // Player versions released until January 12, 2021
        private static List<PatchableBinary> binaries = new List<PatchableBinary>() {
            new PatchableBinary(
                "Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_32_0_0_465.dll", "32,0,0,465", true, 32002616, new List<HexPatch>() {
                new HexPatch(
                    0x2675F0,
                    new byte[] { 0x48, 0x89, 0x6C, 0x24, 0x18, 0x48, 0x89, 0x74, 0x24, 0x20 },
                    new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0xC3, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2676F0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x18, 0x48, 0x89, 0x6C, 0x24, 0x20 },
                    new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0xC3, 0x90, 0x90, 0x90, 0x90 }
                )},
                new List<string>() {
                    Path.Combine(GetLocalAppdata(), "Google", "Chrome", "User Data", "PepperFlash", "32.0.0.465", "pepflashplayer.dll"),
                    Path.Combine(GetLocalAppdata(), "Microsoft", "Edge", "User Data", "PepperFlash", "32.0.0.465", "pepflashplayer.dll")
                }
            ),
            new PatchableBinary(
                "Firefox 64-bit Plugin (NPAPI)", "NPSWF64_32_0_0_465.dll", "32,0,0,465", true, 26911800, new List<HexPatch>() {
                new HexPatch(
                    0x378550,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x3930B2,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x39E340,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x4E397B,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                )
            }),
            new PatchableBinary(
                "IE 64-bit Plugin (ActiveX)", "32,0,0,445", true, 28979096, new List<string>() { "Flash.ocx", "Flash64_32_0_0_445.ocx" }, new List<HexPatch>() {
                new HexPatch(
                    0x2A666C,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x2C32FB,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x2CF7DC,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                )
            }),
            new PatchableBinary(
                "Firefox 32-bit Plugin (NPAPI)", "NPSWF32_32_0_0_465.dll", "32,0,0,465", false, 20404792, new List<HexPatch>() {
                new HexPatch(
                    0x2D29C5,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x2E8F8A,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x2F2771,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x3EE74D,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                )
            }),
            new PatchableBinary(
                "IE 32-bit Plugin (ActiveX)", "32,0,0,445", false, 22874520, new List<string>() { "Flash.ocx", "Flash32_32_0_0_445.ocx" }, new List<HexPatch>() {
                new HexPatch(
                    0x2A0C9B,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x2BA490,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x2C53CF,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: NPSWF32_32_0_0_465.dll
                "Firefox 32-bit Debug Plugin (NPAPI)", "32,0,0,465", false, 21235768, new List<HexPatch>() {
                new HexPatch(
                    0x2E44E5,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x2FBAEA,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x305E98,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x40E363,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libflashplayer.so
                "Linux 64-bit Firefox Plugin (NPAPI)", "32,0,0,465", true, 16653576, new List<HexPatch>() {
                new HexPatch(
                    0x591EB5,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x84, 0x9B, 0x01 },
                    new byte[] { 0x90, 0x90, 0xE9, 0x9C, 0x01, 0x00 }
                ),
                new HexPatch(
                    0x593BB0,
                    new byte[] { 0x84, 0xD2, 0x75, 0x8C },
                    new byte[] { 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x5B59A0,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x85, 0x88, 0x01, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x63E55E,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x84, 0xB6, 0x01 },
                    new byte[] { 0x90, 0x90, 0xE9, 0xB7, 0x01, 0x00 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libpepflashplayer.so
                "Linux 64-bit Chrome Plugin (PPAPI)", "32,0,0,465", true, 19509216, new List<HexPatch>() {
                new HexPatch(
                    0x24B62A,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x85, 0x2E, 0x02, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x6AA580,
                    new byte[] { 0x84, 0xD2, 0x0F, 0x85, 0x70, 0xFF, 0xFF, 0xFF },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x6BB028,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x84, 0x98, 0x01 },
                    new byte[] { 0x90, 0x90, 0xE9, 0x99, 0x01, 0x00 }
                ),
                new HexPatch(
                    0x6D17C8,
                    new byte[] { 0x84, 0xC0 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x6D17D0,
                    new byte[] { 0x0F, 0x85, 0xAA, 0x01, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libflashplayer.so
                "Linux 32-bit Firefox Plugin (NPAPI)", "32,0,0,465", false, 15093756, new List<HexPatch>() {
                new HexPatch(
                    0x4AC0D1,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x84, 0xAC, 0x01 },
                    new byte[] { 0x90, 0x90, 0xE9, 0xAD, 0x01, 0x00 }
                ),
                new HexPatch(
                    0x4ADDCE,
                    new byte[] { 0x84, 0xD2, 0x75, 0x86 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4CFD1B,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x85, 0x9C, 0x01, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x554770,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x84, 0xAA, 0x01 },
                    new byte[] { 0x90, 0x90, 0xE9, 0xAB, 0x01, 0x00 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libpepflashplayer.so
                "Linux 32-bit Chrome Plugin (PPAPI)", "32,0,0,465", false, 16000716, new List<HexPatch>() {
                new HexPatch(
                    0xCA080,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x85, 0x46, 0x02, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x53F5C4,
                    new byte[] { 0x84, 0xD2, 0x0F, 0x85, 0x69, 0xFF, 0xFF, 0xFF },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x551055,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x84, 0x48, 0x01 },
                    new byte[] { 0x90, 0x90, 0xE9, 0x49, 0x01, 0x00 }
                ),
                new HexPatch(
                    0x568EE7,
                    new byte[] { 0x84, 0xC0 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x568EEC,
                    new byte[] { 0x0F, 0x85, 0x9A, 0x01, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Mac 64-bit Chrome Plugin (PPAPI)", "32,0,0,465", true, 27794224, new List<HexPatch>() {
                new HexPatch(
                    0x4FF9C,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x5A6AF2,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x5A750A,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x85, 0xC6, 0x02, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x5B9F93,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Mac 64-bit Firefox Plugin (NPAPI)", "32,0,0,465", true, 29359280, new List<HexPatch>() {
                new HexPatch(
                    0x3E573B,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x3E62CD,
                    new byte[] { 0x84, 0xC0, 0x0F, 0x85, 0x2D, 0x03, 0x00, 0x00 },
                    new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3FA867,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                ),
                new HexPatch(
                    0x47FA13,
                    new byte[] { 0x84, 0xC0, 0x74 },
                    new byte[] { 0x90, 0x90, 0xEB }
                )
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_118.dll", "34,0,0,118", false, 8908984, new List<HexPatch>() {
                new HexPatch(
                    0x257E18,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_118.dll", "34,0,0,118", true, 15982776, new List<HexPatch>() {
                new HexPatch(
                    0x414A30,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_118.dll", "34,0,0,118", false, 9824952, new List<HexPatch>() {
                new HexPatch(
                    0x295F24,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_118.dll", "34,0,0,118", true, 12080824, new List<HexPatch>() {
                new HexPatch(
                    0x33B7E0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_118.ocx", "34,0,0,118", false, 11804344, new List<HexPatch>() {
                new HexPatch(
                    0x15E8C0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_118.ocx", "34,0,0,118", true, 13763768, new List<HexPatch>() {
                new HexPatch(
                    0x15F0D4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libflashplayer.so
                "Chinese Linux 32-bit Firefox Plugin (NPAPI)", "34,0,0,118", false, 15663680, new List<HexPatch>() {
                new HexPatch(
                    0x4F3F00,
                    new byte[] { 0x56, 0x53, 0x83, 0xEC, 0x14, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libflashplayer.so
                "Chinese Linux 64-bit Firefox Plugin (NPAPI)", "34,0,0,118", true, 17143256, new List<HexPatch>() {
                new HexPatch(
                    0x608660,
                    new byte[] { 0x55, 0x48, 0x89, 0xF5, 0x53, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libpepflashplayer.so
                "Chinese Linux 32-bit Chrome Plugin (PPAPI)", "34,0,0,118", false, 13503988, new List<HexPatch>() {
                new HexPatch(
                    0x5AB0F0,
                    new byte[] { 0x55, 0x89, 0xE5, 0x57, 0x56, 0x53 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libpepflashplayer.so
                "Chinese Linux 64-bit Chrome Plugin (PPAPI)", "34,0,0,118", true, 16742160, new List<HexPatch>() {
                new HexPatch(
                    0x704450,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0xF0, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                "Generic Flash Player Binary", null, false, -1, new List<HexPatch>() {
                new HexPatch(
                    -1,
                    new byte[] { 0x00, 0x00, 0x40, 0x46, 0x3E, 0x6F, 0x77, 0x42 },
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x7F }
                )
            })
        };

        private static string executionOptionsPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\";
        private static List<string> telemetryApps = new List<string>() { "FlashCenterService.exe" };

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

        private static string GetLocalAppdata() {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
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

            IntPtr wow64Value = IntPtr.Zero;

            // Disable file system indirection (otherwise we can't read System32)
            Wow64DisableWow64FsRedirection(ref wow64Value);

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

            try {
                ChangeServiceStartMode("FlashCenterService", "Disabled");
            } catch (Exception e) {
                // It's fine if this doesn't work
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
            List<string> incompatibleSize = new List<string>();
            List<string> ownershipFailed = new List<string>();
            List<string> locked = new List<string>();
            List<string> errors = new List<string>();

            string backupFolder = Path.Combine(Environment.CurrentDirectory, "Backup");
            bool madeBackupFolder = false;

            foreach (PatchableBinary binary in binaries) {
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
                        if (!File.Exists(path)) {
                            continue;
                        }

                        found = true;
                        string version = GetVersion(path);

                        if (!binary.GetVersion().Equals(version)) {
                            // We've encountered an incompatible version.
                            incompatibleVersion.Add(string.Format("{0} ({1})", name, version));
                            continue;
                        }

                        long size = new FileInfo(path).Length;

                        if (binary.GetFileSize() != size) {
                            // This file's size does not match the expected file size.
                            incompatibleSize.Add(name);
                            continue;
                        }

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

                            patched.Add(name);
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
                    notFound.Add(name);
                }
            }

            // Enable file system indirection.
            Wow64RevertWow64FsRedirection(wow64Value);

            StringBuilder report = new StringBuilder();
            MessageBoxIcon icon = MessageBoxIcon.Information;

            AppendItems(report, "Successfully patched these plugins:", patched);
            AppendItems(report, "These plugins have already been patched:", alreadyPatched);
            AppendItems(report, "These plugins have not been found on your system:", notFound);
            AppendItems(report, "These plugins are incompatible with the patch because their version is outdated:", incompatibleVersion);
            AppendItems(report, "These plugins are incompatible with the patch because their file size does not match:", incompatibleSize);
            AppendItems(report, "These plugins could not be patched because their respective browser is currently open:", locked);
            AppendItems(report, "Caught exceptions:", errors);

            if (incompatibleVersion.Count > 0 || incompatibleSize.Count > 0 || locked.Count > 0 || errors.Count > 0) {
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

                foreach (PatchableBinary binary in binaries) {
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

        public static void RestoreAll() {
            if (MessageBox.Show("Are you sure you want to restore your Flash Plugin backups?", "FlashPatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            try {
                ChangeServiceStartMode("FlashCenterService", "Automatic");
            } catch (Exception e) {
                // It's fine if this doesn't work
            }

            try {
                foreach (string telemetryApp in telemetryApps) {
                    RestoreExecution(telemetryApp);
                }
            } catch (Exception e) {
                ShowError(string.Format("Could not re-enable Flash Player telemetry! Are you running this application as administrator?\n\n{0}", e.Message));
            }

            string backupFolder = Path.Combine(Environment.CurrentDirectory, "Backup");

            if (!Directory.Exists(backupFolder)) {
                ShowError("No backups are currently available.");
                return;
            }

            IntPtr wow64Value = IntPtr.Zero;

            // Disable file system indirection (otherwise we can't read System32)
            Wow64DisableWow64FsRedirection(ref wow64Value);

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

            foreach (PatchableBinary binary in binaries) {
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

            // Enable file system indirection.
            Wow64RevertWow64FsRedirection(wow64Value);

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
