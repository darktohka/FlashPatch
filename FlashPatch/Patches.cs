using System;
using System.Collections.Generic;
using System.IO;

namespace FlashPatch {
    public class Patches {
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
                // WARNING: A custom patch is not available for this version.
                // FlashPatch will attempt to apply the generic Flash Player patch.
                "IE 64-bit Plugin (ActiveX)", "Flash64_32_0_0_465.ocx", "32,0,0,465", true, -1, new List<HexPatch>() {
                new HexPatch(
                    -1,
                    new byte[] { 0x00, 0x00, 0x40, 0x46, 0x3E, 0x6F, 0x77, 0x42 },
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x7F }
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
                // WARNING: A custom patch is not available for this version.
                // FlashPatch will attempt to apply the generic Flash Player patch.
                "IE 32-bit Plugin (ActiveX)", "Flash32_32_0_0_465.ocx", "32,0,0,465", false, -1, new List<HexPatch>() {
                new HexPatch(
                    -1,
                    new byte[] { 0x00, 0x00, 0x40, 0x46, 0x3E, 0x6F, 0x77, 0x42 },
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x7F }
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
                "Linux 64-bit Chrome Plugin (Pepper)", "32,0,0,465", true, 19509216, new List<HexPatch>() {
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
                "Linux 32-bit Chrome Plugin (Pepper)", "32,0,0,465", false, 16000716, new List<HexPatch>() {
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
                "Mac 64-bit Chrome Plugin (Pepper)", "32,0,0,465", true, 27794224, new List<HexPatch>() {
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
                "Chinese Linux 32-bit Chrome Plugin (Pepper)", "34,0,0,118", false, 13503988, new List<HexPatch>() {
                new HexPatch(
                    0x5AB0F0,
                    new byte[] { 0x55, 0x89, 0xE5, 0x57, 0x56, 0x53 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libpepflashplayer.so
                "Chinese Linux 64-bit Chrome Plugin (Pepper)", "34,0,0,118", true, 16742160, new List<HexPatch>() {
                new HexPatch(
                    0x704450,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0xF0, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,118", true, 15254512, new List<HexPatch>() {
                new HexPatch(
                    0x5B9CA0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                )
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,118", true, 17144384, new List<HexPatch>() {
                new HexPatch(
                    0x4A87A0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_137.dll", "34,0,0,137", false, 8910520, new List<HexPatch>() {
                new HexPatch(
                    0x13E8EF,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13E8F8,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x257DBD,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25F545,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x25F54A,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x260D9E,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_137.dll", "34,0,0,137", true, 15984312, new List<HexPatch>() {
                new HexPatch(
                    0x213737,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x414AD0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x421250,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4239D2,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x4270ED,
                    new byte[] { 0x75, 0x52 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x427102,
                    new byte[] { 0x75, 0x3D },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_137.dll", "34,0,0,137", false, 9825464, new List<HexPatch>() {
                new HexPatch(
                    0x1364CB,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137D52,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x262ED1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x262EDA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2960ED,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_137.dll", "34,0,0,137", true, 12082360, new List<HexPatch>() {
                new HexPatch(
                    0x197100,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199220,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FBCF9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FBD02,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33B8A4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_137.ocx", "34,0,0,137", false, 11805368, new List<HexPatch>() {
                new HexPatch(
                    0x7A090,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C266,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x15EA40,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x162560,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x0C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23952B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x239534,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_137.ocx", "34,0,0,137", false, 9897144, new List<HexPatch>() {
                new HexPatch(
                    0x135F12,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13778A,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x261631,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26163A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2943F9,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_137.ocx", "34,0,0,137", true, 13764280, new List<HexPatch>() {
                new HexPatch(
                    0x80800,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x8297E,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x15ED44,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x2451D0,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2451D9,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_137.ocx", "34,0,0,137", true, 12526264, new List<HexPatch>() {
                new HexPatch(
                    0x196B14,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198CD4,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FCC79,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FCC82,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33C530,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libpepflashplayer.so
                "Chinese Linux 32-bit Chrome Plugin (Pepper)", "34,0,0,137", false, 13504308, new List<HexPatch>() {
                new HexPatch(
                    0x55BE6C,
                    new byte[] { 0x74, 0x55 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x55BEA0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x55BEA9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x5AADC0,
                    new byte[] { 0x55, 0x89, 0xE5, 0x57, 0x56, 0x53 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libpepflashplayer.so
                "Chinese Linux 64-bit Chrome Plugin (Pepper)", "34,0,0,137", true, 16746864, new List<HexPatch>() {
                new HexPatch(
                    0x6B916D,
                    new byte[] { 0x74, 0x59 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x6B919F,
                    new byte[] { 0x74, 0x27 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x6B91A8,
                    new byte[] { 0x75, 0x1E },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x704B30,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0xF0, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libflashplayer.so
                "Chinese Linux 32-bit Firefox Plugin (NPAPI)", "34,0,0,137", false, 15664000, new List<HexPatch>() {
                new HexPatch(
                    0x4CF329,
                    new byte[] { 0x74, 0x65 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4CF35D,
                    new byte[] { 0x74, 0x31 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4CF366,
                    new byte[] { 0x75, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4F3F00,
                    new byte[] { 0x56, 0x53, 0x83, 0xEC, 0x14, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: libflashplayer.so
                "Chinese Linux 64-bit Firefox Plugin (NPAPI)", "34,0,0,137", true, 17143864, new List<HexPatch>() {
                new HexPatch(
                    0x5E3069,
                    new byte[] { 0x74, 0x6D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x5E309B,
                    new byte[] { 0x74, 0x3B },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x5E30A4,
                    new byte[] { 0x75, 0x32 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x608670,
                    new byte[] { 0x55, 0x48, 0x89, 0xF5, 0x53, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,137", true, 15254512, new List<HexPatch>() {
                new HexPatch(
                    0x5B9BE0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,137", true, 17148528, new List<HexPatch>() {
                new HexPatch(
                    0x4A87B0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_155.dll", "34,0,0,155", true, 16003776, new List<HexPatch>() {
                new HexPatch(
                    0x21404E,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x214057,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x415430,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x421E60,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4245E2,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x427CFD,
                    new byte[] { 0x75, 0x52 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x427D12,
                    new byte[] { 0x75, 0x3D },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_155.dll", "34,0,0,155", true, 16952000, new List<HexPatch>() {
                new HexPatch(
                    0x325809,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x325812,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x437160,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44C310,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44EA82,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x45219D,
                    new byte[] { 0x75, 0x52 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4521B2,
                    new byte[] { 0x75, 0x3D },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_155.dll", "34,0,0,155", false, 8925376, new List<HexPatch>() {
                new HexPatch(
                    0x13E6FF,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13E708,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x257AE9,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25F319,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x25F31E,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x260B72,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_155.dll", "34,0,0,155", false, 9530560, new List<HexPatch>() {
                new HexPatch(
                    0x1984B5,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1984BE,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25B109,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x267A97,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x267A9C,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26930C,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_155.dll", "34,0,0,155", true, 12097216, new List<HexPatch>() {
                new HexPatch(
                    0x197258,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199378,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FBD79,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FBD82,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33BA30,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_155.dll", "34,0,0,155", true, 13086400, new List<HexPatch>() {
                new HexPatch(
                    0x197964,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199A78,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3149FF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x314A08,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3561A4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_155.dll", "34,0,0,155", false, 9839808, new List<HexPatch>() {
                new HexPatch(
                    0x1365CD,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137E7C,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x262EF1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x262EFA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x296143,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_155.dll", "34,0,0,155", false, 10661056, new List<HexPatch>() {
                new HexPatch(
                    0x13533A,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136BE9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2753F0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2753F9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A9B3F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 10-specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_155.ocx", "34,0,0,155", false, 13779648, new List<HexPatch>() {
                new HexPatch(
                    0x80A60,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82BDE,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x15F0E8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x245470,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x245479,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_155.ocx", "34,0,0,155", true, 12541120, new List<HexPatch>() {
                new HexPatch(
                    0x196B24,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198CE4,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FCD09,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FCD12,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33C6F4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_155.ocx", "34,0,0,155", true, 13518016, new List<HexPatch>() {
                new HexPatch(
                    0x1968DC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198A90,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x313F2F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x313F38,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x355410,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 10-specific
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_155.ocx", "34,0,0,155", false, 11819712, new List<HexPatch>() {
                new HexPatch(
                    0x79E40,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C016,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x15E790,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x2390BB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2390C4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_155.ocx", "34,0,0,155", false, 9912512, new List<HexPatch>() {
                new HexPatch(
                    0x135E09,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1376EC,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x261111,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26111A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x293F00,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_155.ocx", "34,0,0,155", false, 10729152, new List<HexPatch>() {
                new HexPatch(
                    0x134CA1,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136535,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x273950,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x273959,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A7BBD,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,155", true, 15297104, new List<HexPatch>() {
                new HexPatch(
                    0x56BF39,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BF76,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BF7F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56BF84,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CA150,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,155", true, 16792288, new List<HexPatch>() {
                new HexPatch(
                    0x3CA0B9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA0F6,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA0FF,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA104,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B4390,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,155", false, 10726080, new List<HexPatch>() {
                new HexPatch(
                    0x62C04,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62C0D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9C92,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,155", false, 11545792, new List<HexPatch>() {
                new HexPatch(
                    0x187AF3,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x187AFC,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326BA3,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_164.dll", "34,0,0,164", true, 16007408, new List<HexPatch>() {
                new HexPatch(
                    0x21406E,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x214077,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x415450,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x421EE0,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x424672,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x427D6D,
                    new byte[] { 0x75, 0x52 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x427D82,
                    new byte[] { 0x75, 0x3D },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_164.dll", "34,0,0,164", true, 16956144, new List<HexPatch>() {
                new HexPatch(
                    0x325829,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x325832,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x437180,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44C390,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44EB12,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x45220D,
                    new byte[] { 0x75, 0x52 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x452222,
                    new byte[] { 0x75, 0x3D },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_164.dll", "34,0,0,164", false, 8927472, new List<HexPatch>() {
                new HexPatch(
                    0x13EA3F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13EA48,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x257E58,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25F6F2,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x25F6F7,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x260F7F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_164.dll", "34,0,0,164", false, 9532656, new List<HexPatch>() {
                new HexPatch(
                    0x198425,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x19842E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25AFED,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x267A9A,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x267A9F,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2692D8,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_164.dll", "34,0,0,164", true, 12099312, new List<HexPatch>() {
                new HexPatch(
                    0x1970A0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19926C,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FC449,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FC452,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33C008,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_164.dll", "34,0,0,164", true, 13089008, new List<HexPatch>() {
                new HexPatch(
                    0x197A0C,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199BCC,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31509F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3150A8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x356864,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_164.dll", "34,0,0,164", false, 9841392, new List<HexPatch>() {
                new HexPatch(
                    0x13653D,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137D8D,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2634E1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2634EA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x29672E,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_164.dll", "34,0,0,164", false, 10662640, new List<HexPatch>() {
                new HexPatch(
                    0x135347,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136BBF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x275A20,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x275A29,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AA1D8,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_164.ocx", "34,0,0,164", true, 12543216, new List<HexPatch>() {
                new HexPatch(
                    0x196DC4,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198F90,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FD339,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FD342,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33CBB8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_164.ocx", "34,0,0,164", true, 13520624, new List<HexPatch>() {
                new HexPatch(
                    0x196A54,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198C14,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3146FF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x314708,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x355AFC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_164.ocx", "34,0,0,164", false, 9914096, new List<HexPatch>() {
                new HexPatch(
                    0x135D7E,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1375CE,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x261571,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26157A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2943C6,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_164.ocx", "34,0,0,164", false, 10730736, new List<HexPatch>() {
                new HexPatch(
                    0x134DBC,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13666B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x273D90,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x273D99,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A811D,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,164", true, 15297104, new List<HexPatch>() {
                new HexPatch(
                    0x56BF49,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BF86,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BF8F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56BF94,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CA160,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,164", true, 16792288, new List<HexPatch>() {
                new HexPatch(
                    0x3CA0B9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA0F6,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA0FF,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA104,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B4390,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,164", false, 10725616, new List<HexPatch>() {
                new HexPatch(
                    0x62C94,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62C9D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9F7F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,164", false, 11545840, new List<HexPatch>() {
                new HexPatch(
                    0x187AA3,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x187AAC,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326D16,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_175.dll", "34,0,0,175", true, 16054512, new List<HexPatch>() {
                new HexPatch(
                    0x21420E,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x214217,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x415E40,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x422970,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x425212,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_175.dll", "34,0,0,175", true, 17003760, new List<HexPatch>() {
                new HexPatch(
                    0x3259D9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3259E2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x437B80,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44CE30,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44F6D2,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_175.dll", "34,0,0,175", false, 8968944, new List<HexPatch>() {
                new HexPatch(
                    0x13E49F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13E4A8,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258248,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25FB2F,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x25FB34,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2613C4,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_175.dll", "34,0,0,175", false, 9574640, new List<HexPatch>() {
                new HexPatch(
                    0x1981B5,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1981BE,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25B3F3,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x267FC8,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x267FCD,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x269829,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_175.dll", "34,0,0,175", true, 12142832, new List<HexPatch>() {
                new HexPatch(
                    0x196F80,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1990B8,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FD1A9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FD1B2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33CE08,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_175.dll", "34,0,0,175", true, 13132528, new List<HexPatch>() {
                new HexPatch(
                    0x1979C0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199B8C,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x315BBF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x315BC8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3574C0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_175.dll", "34,0,0,175", false, 9882864, new List<HexPatch>() {
                new HexPatch(
                    0x13631C,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137C17,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x263F61,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x263F6A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x29715B,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_175.dll", "34,0,0,175", false, 10704624, new List<HexPatch>() {
                new HexPatch(
                    0x1353E3,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136C76,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2765A0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2765A9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AAD20,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_175.ocx", "34,0,0,175", true, 12587248, new List<HexPatch>() {
                new HexPatch(
                    0x196B90,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198D68,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FE5F9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FE602,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33DF20,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_175.ocx", "34,0,0,175", true, 13564656, new List<HexPatch>() {
                new HexPatch(
                    0x196A78,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198C44,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31580F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x315818,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x356C2C,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_175.ocx", "34,0,0,175", false, 9955568, new List<HexPatch>() {
                new HexPatch(
                    0x135E12,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1376CA,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2624C1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2624CA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x295265,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_175.ocx", "34,0,0,175", false, 10771184, new List<HexPatch>() {
                new HexPatch(
                    0x134ECA,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13671A,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x274EE0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x274EE9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A9172,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,175", true, 15301248, new List<HexPatch>() {
                new HexPatch(
                    0x56B6F9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56B736,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56B73F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56B744,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5C98F0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,175", true, 16792288, new List<HexPatch>() {
                new HexPatch(
                    0x3CA1C9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA206,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA20F,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA214,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B44A0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,175", false, 10730736, new List<HexPatch>() {
                new HexPatch(
                    0x62C84,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62C8D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9FCC,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,175", false, 11549936, new List<HexPatch>() {
                new HexPatch(
                    0x187B53,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x187B5C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326E33,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
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

        public static List<PatchableBinary> GetBinaries() {
            return binaries;
        }

        private static string GetLocalAppdata() {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}
