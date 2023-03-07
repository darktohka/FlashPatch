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
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_32_sa.exe
                "Standalone Flash Player", "32,0,0,465", false, 15983672, new List<HexPatch>() {
                new HexPatch(
                    0xAE894,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0xAE89D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2389EA,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_32_sa_debug.exe
                "Standalone Flash Player", "32,0,0,465", false, 16811576, new List<HexPatch>() {
                new HexPatch(
                    0x1D7BB3,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D7BBC,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x39B14E,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
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
                // Windows 8+ specific plugin
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
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_184.dll", "34,0,0,184", true, 16080112, new List<HexPatch>() {
                new HexPatch(
                    0x217AB9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x217AC2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x419900,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x426730,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x428EC5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_184.dll", "34,0,0,184", true, 17028336, new List<HexPatch>() {
                new HexPatch(
                    0x329281,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x32928A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x43B640,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x450BF0,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x453375,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_184.dll", "34,0,0,184", false, 8974576, new List<HexPatch>() {
                new HexPatch(
                    0x13F06F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F078,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258AE4,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x2604B3,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x2604B8,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x261D78,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_184.dll", "34,0,0,184", false, 9579248, new List<HexPatch>() {
                new HexPatch(
                    0x198275,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x19827E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25B5D2,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268270,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x268275,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x269B70,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_184.dll", "34,0,0,184", true, 12145904, new List<HexPatch>() {
                new HexPatch(
                    0x1973E8,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1995BF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FD7C9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FD7D2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33D4B0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_184.dll", "34,0,0,184", true, 13135600, new List<HexPatch>() {
                new HexPatch(
                    0x197B54,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199D1F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31659F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3165A8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x357DD8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_184.dll", "34,0,0,184", false, 9886448, new List<HexPatch>() {
                new HexPatch(
                    0x1365C5,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137E2E,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2643F1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2643FA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2976EF,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_184.dll", "34,0,0,184", false, 10707184, new List<HexPatch>() {
                new HexPatch(
                    0x135390,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136BED,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x276AB0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x276AB9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AB1CF,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_184.ocx", "34,0,0,184", true, 12589808, new List<HexPatch>() {
                new HexPatch(
                    0x196A3C,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198C13,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FE7D9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FE7E2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E090,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_184.ocx", "34,0,0,184", true, 13829872, new List<HexPatch>() {
                new HexPatch(
                    0x808F0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82A81,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160CF8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x247070,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x247079,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_184.ocx", "34,0,0,184", true, 13568240, new List<HexPatch>() {
                new HexPatch(
                    0x196B98,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198D63,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3158BF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3158C8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x356C80,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_184.ocx", "34,0,0,184", true, 14685936, new List<HexPatch>() {
                new HexPatch(
                    0x160C48,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24D601,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24D60A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_184.ocx", "34,0,0,184", false, 9958640, new List<HexPatch>() {
                new HexPatch(
                    0x135CCF,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137538,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x262A91,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x262A9A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x295995,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_184.ocx", "34,0,0,184", false, 11869424, new List<HexPatch>() {
                new HexPatch(
                    0x7A120,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C359,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160B10,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23B34B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23B354,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_184.ocx", "34,0,0,184", false, 10775280, new List<HexPatch>() {
                new HexPatch(
                    0x134D6B,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1365F0,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x274D90,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x274D99,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A9019,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_184.ocx", "34,0,0,184", false, 12748016, new List<HexPatch>() {
                new HexPatch(
                    0x7A770,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C9E9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x174930,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x2575EB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2575F4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,184", true, 15305408, new List<HexPatch>() {
                new HexPatch(
                    0x56BDE9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BE26,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BE2F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56BE34,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CA000,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,184", true, 16792304, new List<HexPatch>() {
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
                    0x4B44B0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,184", false, 10730736, new List<HexPatch>() {
                new HexPatch(
                    0x62D14,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62D1D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9DAC,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,184", false, 11550448, new List<HexPatch>() {
                new HexPatch(
                    0x187893,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x18789C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326BE3,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_192.dll", "34,0,0,192", true, 16071408, new List<HexPatch>() {
                new HexPatch(
                    0x217379,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x217382,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4182C0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x425290,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x427A25,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_192.dll", "34,0,0,192", true, 17021168, new List<HexPatch>() {
                new HexPatch(
                    0x328B31,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328B3A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x439F80,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44F6D0,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x451E55,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_192.dll", "34,0,0,192", false, 8974576, new List<HexPatch>() {
                new HexPatch(
                    0x13F37F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F388,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25855D,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x2600E2,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x2600E7,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2619A3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_192.dll", "34,0,0,192", false, 9578736, new List<HexPatch>() {
                new HexPatch(
                    0x198F25,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198F2E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25B81D,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268478,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26847D,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x269D09,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_192.dll", "34,0,0,192", true, 12145904, new List<HexPatch>() {
                new HexPatch(
                    0x1970F0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199227,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FD679,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FD682,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33D2C0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_192.dll", "34,0,0,192", true, 13136112, new List<HexPatch>() {
                new HexPatch(
                    0x197AEC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199C17,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31636F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x316378,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x357B6C,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_192.dll", "34,0,0,192", false, 9886960, new List<HexPatch>() {
                new HexPatch(
                    0x1366D6,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137F73,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x264571,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26457A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x29777B,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_192.dll", "34,0,0,192", false, 10707696, new List<HexPatch>() {
                new HexPatch(
                    0x1351BA,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136AB6,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2769F0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2769F9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AB247,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_192.ocx", "34,0,0,192", true, 12590320, new List<HexPatch>() {
                new HexPatch(
                    0x196B2C,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198D03,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FE7B9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FE7C2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E094,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_192.ocx", "34,0,0,192", true, 13830384, new List<HexPatch>() {
                new HexPatch(
                    0x808E0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82A71,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160DB4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x247320,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x247329,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_192.ocx", "34,0,0,192", true, 13568752, new List<HexPatch>() {
                new HexPatch(
                    0x1969BC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198B87,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x315BDF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x315BE8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x356FEC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_192.ocx", "34,0,0,192", true, 14686448, new List<HexPatch>() {
                new HexPatch(
                    0x160D14,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24D9F1,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24D9FA,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_192.ocx", "34,0,0,192", false, 9959152, new List<HexPatch>() {
                new HexPatch(
                    0x135D13,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13757C,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x262941,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26294A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2956F6,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_192.ocx", "34,0,0,192", false, 11869936, new List<HexPatch>() {
                new HexPatch(
                    0x7A080,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C2B9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160A60,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23B5BB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23B5C4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_192.ocx", "34,0,0,192", false, 10775792, new List<HexPatch>() {
                new HexPatch(
                    0x134D9D,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1365FA,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x274E40,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x274E49,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A908B,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_192.ocx", "34,0,0,192", false, 12748016, new List<HexPatch>() {
                new HexPatch(
                    0x7A7B0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CA39,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1747D0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x2573CB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2573D4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,192", true, 15301296, new List<HexPatch>() {
                new HexPatch(
                    0x56BB39,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BB76,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BB7F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56BB84,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5C9E60,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,192", true, 16792304, new List<HexPatch>() {
                new HexPatch(
                    0x3CA109,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA146,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA14F,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA154,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B44E0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,192", false, 10731248, new List<HexPatch>() {
                new HexPatch(
                    0x62CC4,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62CCD,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9E05,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,192", false, 11550448, new List<HexPatch>() {
                new HexPatch(
                    0x187A43,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x187A4C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326C2C,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_201.dll", "34,0,0,201", true, 16071408, new List<HexPatch>() {
                new HexPatch(
                    0x217379,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x217382,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4182C0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x425290,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x427A25,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_201.dll", "34,0,0,201", true, 17021168, new List<HexPatch>() {
                new HexPatch(
                    0x328B31,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328B3A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x439F80,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44F6D0,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x451E55,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_201.dll", "34,0,0,201", false, 8974576, new List<HexPatch>() {
                new HexPatch(
                    0x13F37F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F388,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25855D,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x2600E2,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x2600E7,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2619A3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_201.dll", "34,0,0,201", false, 9578736, new List<HexPatch>() {
                new HexPatch(
                    0x198F15,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198F1E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25B81D,
                    new byte[] { 0x56, 0x8B, 0xF1, 0x57, 0x8B, 0x7C },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268478,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26847D,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x269D09,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_201.dll", "34,0,0,201", true, 12145904, new List<HexPatch>() {
                new HexPatch(
                    0x1970F0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199227,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FD679,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FD682,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33D2C0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_201.dll", "34,0,0,201", true, 13136112, new List<HexPatch>() {
                new HexPatch(
                    0x197AEC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199C17,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31636F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x316378,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x357B6C,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_201.dll", "34,0,0,201", false, 9886960, new List<HexPatch>() {
                new HexPatch(
                    0x136722,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137FBF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2645E1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2645EA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2977EB,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_201.dll", "34,0,0,201", false, 10707696, new List<HexPatch>() {
                new HexPatch(
                    0x1351CB,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136AC7,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2769E0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2769E9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AB217,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_201.ocx", "34,0,0,201", true, 12590320, new List<HexPatch>() {
                new HexPatch(
                    0x196B1C,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198CF3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FE7A9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FE7B2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E084,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_201.ocx", "34,0,0,201", true, 13830384, new List<HexPatch>() {
                new HexPatch(
                    0x808E0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82A71,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160DB4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x247320,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x247329,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_201.ocx", "34,0,0,201", true, 13568752, new List<HexPatch>() {
                new HexPatch(
                    0x1969AC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198B77,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x315BCF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x315BD8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x356FDC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_201.ocx", "34,0,0,201", true, 14686448, new List<HexPatch>() {
                new HexPatch(
                    0x160D14,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24D9F1,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24D9FA,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_201.ocx", "34,0,0,201", false, 9959152, new List<HexPatch>() {
                new HexPatch(
                    0x135CF6,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13755F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2628F1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2628FA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2956B8,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_201.ocx", "34,0,0,201", false, 11869936, new List<HexPatch>() {
                new HexPatch(
                    0x7A080,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C2B9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160A60,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23B5BB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23B5C4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_201.ocx", "34,0,0,201", false, 10775792, new List<HexPatch>() {
                new HexPatch(
                    0x134DDD,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13663A,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x274EB0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x274EB9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A9100,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_201.ocx", "34,0,0,201", false, 12748528, new List<HexPatch>() {
                new HexPatch(
                    0x7A7F0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CA69,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x174800,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25746B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x257474,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,201", true, 15305440, new List<HexPatch>() {
                new HexPatch(
                    0x56CB39,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB76,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB7F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56CB84,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CAE60,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,201", true, 16792304, new List<HexPatch>() {
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
                    0x4B45A0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,201", false, 10731248, new List<HexPatch>() {
                new HexPatch(
                    0x62CC4,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62CCD,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9E15,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,201", false, 11550448, new List<HexPatch>() {
                new HexPatch(
                    0x1879D3,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1879DC,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326C2C,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_209.dll", "34,0,0,209", true, 16072432, new List<HexPatch>() {
                new HexPatch(
                    0x2173A9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2173B2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x418300,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4252F0,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x427A85,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_209.dll", "34,0,0,209", true, 17021680, new List<HexPatch>() {
                new HexPatch(
                    0x328B61,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328B6A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x439FC0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44F730,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x451EB5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_209.dll", "34,0,0,209", false, 8974576, new List<HexPatch>() {
                new HexPatch(
                    0x13F82F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F838,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258BF1,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x260711,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x260716,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x261F8F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_209.dll", "34,0,0,209", false, 9578224, new List<HexPatch>() {
                new HexPatch(
                    0x198B05,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198B0E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25B36D,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268189,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26818E,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2699FF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_209.dll", "34,0,0,209", true, 12146928, new List<HexPatch>() {
                new HexPatch(
                    0x1971C0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199397,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FDD09,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FDD12,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33D9D0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_209.dll", "34,0,0,209", true, 13136624, new List<HexPatch>() {
                new HexPatch(
                    0x1979C4,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199AEF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31690F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x316918,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x358148,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_209.dll", "34,0,0,209", false, 9887472, new List<HexPatch>() {
                new HexPatch(
                    0x136484,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137D30,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2646B1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2646BA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2978C9,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_209.dll", "34,0,0,209", false, 10708208, new List<HexPatch>() {
                new HexPatch(
                    0x135490,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136D30,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x276B60,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x276B69,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AB2AF,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_209.ocx", "34,0,0,209", true, 12590832, new List<HexPatch>() {
                new HexPatch(
                    0x196DF4,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198FCB,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FE879,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FE882,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E210,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_209.ocx", "34,0,0,209", true, 13831408, new List<HexPatch>() {
                new HexPatch(
                    0x80BC0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82D51,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1612A4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x247870,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x247879,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_209.ocx", "34,0,0,209", true, 13569264, new List<HexPatch>() {
                new HexPatch(
                    0x196B00,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198CCB,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3159FF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x315A08,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x356E70,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_209.ocx", "34,0,0,209", true, 14687472, new List<HexPatch>() {
                new HexPatch(
                    0x1614E4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24DC81,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DC8A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_209.ocx", "34,0,0,209", false, 9959664, new List<HexPatch>() {
                new HexPatch(
                    0x135D69,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137615,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x262B51,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x262B5A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x295AF7,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_209.ocx", "34,0,0,209", false, 11870960, new List<HexPatch>() {
                new HexPatch(
                    0x7A1A0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C3D9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160DC0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23B9BB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23B9C4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_209.ocx", "34,0,0,209", false, 10776304, new List<HexPatch>() {
                new HexPatch(
                    0x134F0B,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13679C,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x275130,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x275139,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A9433,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_209.ocx", "34,0,0,209", false, 12749040, new List<HexPatch>() {
                new HexPatch(
                    0x7A820,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CA69,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x175190,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x257A4B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x257A54,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_211.dll", "34,0,0,211", true, 16072432, new List<HexPatch>() {
                new HexPatch(
                    0x217399,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2173A2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x4182F0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4252E0,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x427A75,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_211.dll", "34,0,0,211", true, 17021680, new List<HexPatch>() {
                new HexPatch(
                    0x328B51,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328B5A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x439FB0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x44F720,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x451EA5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_211.dll", "34,0,0,211", false, 8974576, new List<HexPatch>() {
                new HexPatch(
                    0x13F83F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F848,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258BD1,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x260723,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x260728,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x261FA1,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_211.dll", "34,0,0,211", false, 9578736, new List<HexPatch>() {
                new HexPatch(
                    0x198B05,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198B0E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25B36D,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268189,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26818E,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2699FF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_211.dll", "34,0,0,211", true, 12146928, new List<HexPatch>() {
                new HexPatch(
                    0x1971C0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199397,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FDD09,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FDD12,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33D9D0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_211.dll", "34,0,0,211", true, 13136624, new List<HexPatch>() {
                new HexPatch(
                    0x1979D4,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199AFF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31691F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x316928,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x358158,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_211.dll", "34,0,0,211", false, 9887472, new List<HexPatch>() {
                new HexPatch(
                    0x136446,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137CF2,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x264671,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26467A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x29788A,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_211.dll", "34,0,0,211", false, 10708208, new List<HexPatch>() {
                new HexPatch(
                    0x1354CE,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136D6E,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x276DF0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x276DF9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AB53F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_211.ocx", "34,0,0,211", true, 12590832, new List<HexPatch>() {
                new HexPatch(
                    0x196DF4,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198FCB,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FE879,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FE882,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E210,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_211.ocx", "34,0,0,211", true, 13831408, new List<HexPatch>() {
                new HexPatch(
                    0x80BC0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82D51,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1612A4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x247870,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x247879,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_211.ocx", "34,0,0,211", true, 13569264, new List<HexPatch>() {
                new HexPatch(
                    0x196B00,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198CCB,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3159FF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x315A08,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x356E70,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_211.ocx", "34,0,0,211", true, 14687472, new List<HexPatch>() {
                new HexPatch(
                    0x1614E4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24DC81,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DC8A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_211.ocx", "34,0,0,211", false, 9959664, new List<HexPatch>() {
                new HexPatch(
                    0x135D39,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1375E5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x262B21,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x262B2A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x295AC7,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_211.ocx", "34,0,0,211", false, 11870960, new List<HexPatch>() {
                new HexPatch(
                    0x7A1A0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C3D9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x160EC0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23B9BB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23B9C4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_211.ocx", "34,0,0,211", false, 10776304, new List<HexPatch>() {
                new HexPatch(
                    0x134F0B,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13679C,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x275130,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x275139,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2A9433,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_211.ocx", "34,0,0,211", false, 12749040, new List<HexPatch>() {
                new HexPatch(
                    0x7A820,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CA69,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x175190,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x257A4B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x257A54,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,211", true, 15305440, new List<HexPatch>() {
                new HexPatch(
                    0x56CB19,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB56,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB5F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56CB64,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CAE40,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,211", true, 16792304, new List<HexPatch>() {
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
                    0x4B45A0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,211", false, 10731760, new List<HexPatch>() {
                new HexPatch(
                    0x62D04,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62D0D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9DF0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,211", false, 11550960, new List<HexPatch>() {
                new HexPatch(
                    0x187963,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x18796C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326C6F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_231.dll", "34,0,0,231", true, 16087792, new List<HexPatch>() {
                new HexPatch(
                    0x217399,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2173A2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x418D30,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x425E60,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4285F5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_231.dll", "34,0,0,231", true, 17036528, new List<HexPatch>() {
                new HexPatch(
                    0x328B51,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328B5A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x43A9E0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x450290,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x452A15,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_231.dll", "34,0,0,231", false, 8986864, new List<HexPatch>() {
                new HexPatch(
                    0x13F56F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F578,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x259036,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x260B62,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x260B67,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26240D,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_231.dll", "34,0,0,231", false, 9591024, new List<HexPatch>() {
                new HexPatch(
                    0x198B85,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198B8E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25BCF7,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268A15,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x268A1A,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26A295,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_231.dll", "34,0,0,231", true, 12159728, new List<HexPatch>() {
                new HexPatch(
                    0x1972FC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19944B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FE7F9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FE802,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E538,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_231.dll", "34,0,0,231", true, 13150448, new List<HexPatch>() {
                new HexPatch(
                    0x197BCC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199D0F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3176DF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3176E8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x359034,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_231.dll", "34,0,0,231", false, 9899760, new List<HexPatch>() {
                new HexPatch(
                    0x136609,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137EEF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x265381,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26538A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2986C8,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_231.dll", "34,0,0,231", false, 10720496, new List<HexPatch>() {
                new HexPatch(
                    0x13557A,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136E2D,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x277B40,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x277B49,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AC280,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_231.ocx", "34,0,0,231", true, 12603632, new List<HexPatch>() {
                new HexPatch(
                    0x196F20,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19910F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FF8E9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FF8F2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33F1D0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_231.ocx", "34,0,0,231", true, 13843696, new List<HexPatch>() {
                new HexPatch(
                    0x80A88,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82C31,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1620B4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x248530,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x248539,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_231.ocx", "34,0,0,231", true, 13582064, new List<HexPatch>() {
                new HexPatch(
                    0x196CC4,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198EA7,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x316E6F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x316E78,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3582E0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_231.ocx", "34,0,0,231", true, 14687472, new List<HexPatch>() {
                new HexPatch(
                    0x1614F4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24DC91,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DC9A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_231.ocx", "34,0,0,231", false, 9971440, new List<HexPatch>() {
                new HexPatch(
                    0x135FB2,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1378A4,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x263891,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26389A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2966D8,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_231.ocx", "34,0,0,231", false, 11884272, new List<HexPatch>() {
                new HexPatch(
                    0x7A240,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C489,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x162170,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23CDEB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23CDF4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_231.ocx", "34,0,0,231", false, 10788080, new List<HexPatch>() {
                new HexPatch(
                    0x134FDB,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1368C1,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x275FA0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x275FA9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AA281,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_231.ocx", "34,0,0,231", false, 12763376, new List<HexPatch>() {
                new HexPatch(
                    0x7A6D0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C919,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x175F50,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x258A1B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258A24,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,231", true, 15305440, new List<HexPatch>() {
                new HexPatch(
                    0x56CB19,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB56,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB5F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56CB64,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CAE40,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,231", true, 16792304, new List<HexPatch>() {
                new HexPatch(
                    0x3CA1D9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA216,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA21F,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA224,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B45B0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,231", false, 10731760, new List<HexPatch>() {
                new HexPatch(
                    0x62D04,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62D0D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9DF0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,231", false, 11550960, new List<HexPatch>() {
                new HexPatch(
                    0x187963,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x18796C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326C8F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_242.dll", "34,0,0,242", true, 16088864, new List<HexPatch>() {
                new HexPatch(
                    0x217399,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2173A2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x418E20,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x425F50,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4286E5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_242.dll", "34,0,0,242", true, 17037088, new List<HexPatch>() {
                new HexPatch(
                    0x328B51,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328B5A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x43AAD0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x450380,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x452B05,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_242.dll", "34,0,0,242", false, 8987424, new List<HexPatch>() {
                new HexPatch(
                    0x13F45F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F468,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258F97,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x260B56,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x260B5B,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2623BA,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_242.dll", "34,0,0,242", false, 9592096, new List<HexPatch>() {
                new HexPatch(
                    0x198975,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x19897E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25BA05,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x26888A,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26888F,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26A0F9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_242.dll", "34,0,0,242", true, 12160800, new List<HexPatch>() {
                new HexPatch(
                    0x197330,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19951F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FEAE9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FEAF2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E6CC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_242.dll", "34,0,0,242", true, 13151008, new List<HexPatch>() {
                new HexPatch(
                    0x197B94,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199D77,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31745F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x317468,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x358D10,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_242.dll", "34,0,0,242", false, 9900832, new List<HexPatch>() {
                new HexPatch(
                    0x1368F7,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x138172,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x265571,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26557A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2987D7,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_242.dll", "34,0,0,242", false, 10721056, new List<HexPatch>() {
                new HexPatch(
                    0x135393,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136C79,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2777E0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2777E9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2ABF08,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_242.ocx", "34,0,0,242", true, 12604704, new List<HexPatch>() {
                new HexPatch(
                    0x196E30,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19901F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FFD59,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FFD62,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33F738,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_242.ocx", "34,0,0,242", true, 13844768, new List<HexPatch>() {
                new HexPatch(
                    0x80BA8,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82D51,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x161EF4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x248510,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x248519,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_242.ocx", "34,0,0,242", true, 13583136, new List<HexPatch>() {
                new HexPatch(
                    0x196CFC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198EDF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x316C8F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x316C98,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3580FC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_242.ocx", "34,0,0,242", true, 14688544, new List<HexPatch>() {
                new HexPatch(
                    0x1614F4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24DC91,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DC9A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_242.ocx", "34,0,0,242", false, 9972512, new List<HexPatch>() {
                new HexPatch(
                    0x135F02,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1377A5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x263AA1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x263AAA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x296938,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_242.ocx", "34,0,0,242", false, 11884832, new List<HexPatch>() {
                new HexPatch(
                    0x7A320,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C569,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x161DC0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23CA8B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23CA94,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_242.ocx", "34,0,0,242", false, 10789152, new List<HexPatch>() {
                new HexPatch(
                    0x134EB4,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136766,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x275F10,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x275F19,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AA29F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_242.ocx", "34,0,0,242", false, 12763936, new List<HexPatch>() {
                new HexPatch(
                    0x7ABA0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CDE9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1762A0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x258DEB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258DF4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,242", true, 15305440, new List<HexPatch>() {
                new HexPatch(
                    0x56CB19,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB56,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB5F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56CB64,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CAE40,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,242", true, 16792304, new List<HexPatch>() {
                new HexPatch(
                    0x3CA1D9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA216,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA21F,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA224,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B45B0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,242", false, 10732832, new List<HexPatch>() {
                new HexPatch(
                    0x62D04,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62D0D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9E80,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,242", false, 11552032, new List<HexPatch>() {
                new HexPatch(
                    0x187933,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x18793C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326C8F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_251.dll", "34,0,0,251", true, 16088864, new List<HexPatch>() {
                new HexPatch(
                    0x217399,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2173A2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x418E30,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x425F60,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4286F5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_251.dll", "34,0,0,251", true, 17037088, new List<HexPatch>() {
                new HexPatch(
                    0x328B51,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328B5A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x43AAE0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x450390,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x452B15,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_251.dll", "34,0,0,251", false, 8987424, new List<HexPatch>() {
                new HexPatch(
                    0x13F55F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F568,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258DDB,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x260A85,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x260A8A,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x262304,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_251.dll", "34,0,0,251", false, 9592096, new List<HexPatch>() {
                new HexPatch(
                    0x198AC5,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198ACE,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25BB59,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268941,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x268946,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26A1B0,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_251.dll", "34,0,0,251", true, 12160800, new List<HexPatch>() {
                new HexPatch(
                    0x197508,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1996F7,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FEDA9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FEDB2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33E9E8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_251.dll", "34,0,0,251", true, 13151008, new List<HexPatch>() {
                new HexPatch(
                    0x197C5C,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199D9F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31777F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x317788,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x358F70,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_251.dll", "34,0,0,251", false, 9900320, new List<HexPatch>() {
                new HexPatch(
                    0x1365B2,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137E55,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x265301,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26530A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x298680,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_251.dll", "34,0,0,251", false, 10721568, new List<HexPatch>() {
                new HexPatch(
                    0x1355F9,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136ED3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x277B10,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x277B19,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AC2D7,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_251.ocx", "34,0,0,251", true, 12604704, new List<HexPatch>() {
                new HexPatch(
                    0x196E9C,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19908B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FFBD9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FFBE2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33F4E4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_251.ocx", "34,0,0,251", true, 13845792, new List<HexPatch>() {
                new HexPatch(
                    0x80B88,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82D31,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1626A8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x248A20,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x248A29,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_251.ocx", "34,0,0,251", true, 13583136, new List<HexPatch>() {
                new HexPatch(
                    0x196BD4,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198DB7,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x316F1F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x316F28,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3583CC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_251.ocx", "34,0,0,251", true, 14688544, new List<HexPatch>() {
                new HexPatch(
                    0x1614F8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24DC91,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DC9A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_251.ocx", "34,0,0,251", false, 9973024, new List<HexPatch>() {
                new HexPatch(
                    0x135FBD,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1378D7,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x263A21,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x263A2A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x296880,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_251.ocx", "34,0,0,251", false, 11885856, new List<HexPatch>() {
                new HexPatch(
                    0x7A330,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C579,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1623F0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23CF9B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23CFA4,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_251.ocx", "34,0,0,251", false, 10789152, new List<HexPatch>() {
                new HexPatch(
                    0x134E72,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136709,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x275FF0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x275FF9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AA323,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_251.ocx", "34,0,0,251", false, 12764448, new List<HexPatch>() {
                new HexPatch(
                    0x7A7F0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CA39,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x175E70,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x258D6B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x258D74,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,251", true, 15305440, new List<HexPatch>() {
                new HexPatch(
                    0x56CB19,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB56,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB5F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56CB64,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CAE40,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,251", true, 16792304, new List<HexPatch>() {
                new HexPatch(
                    0x3CA1D9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA216,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA21F,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA224,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B45C0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,251", false, 10732832, new List<HexPatch>() {
                new HexPatch(
                    0x62D04,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62D0D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1D9E04,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,251", false, 11552032, new List<HexPatch>() {
                new HexPatch(
                    0x187963,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x18796C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326CA3,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_267.dll", "34,0,0,267", true, 16091352, new List<HexPatch>() {
                new HexPatch(
                    0x217269,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x217272,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x418FB0,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x426070,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x428805,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_267.dll", "34,0,0,267", true, 17039576, new List<HexPatch>() {
                new HexPatch(
                    0x328A21,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328A2A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x43AC60,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4504A0,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x452C25,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_267.dll", "34,0,0,267", false, 8989912, new List<HexPatch>() {
                new HexPatch(
                    0x13F82F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F838,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25950F,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x261096,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26109B,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2628F6,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_267.dll", "34,0,0,267", false, 9594584, new List<HexPatch>() {
                new HexPatch(
                    0x198D35,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198D3E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25C206,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268F73,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x268F78,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26A7E6,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_267.dll", "34,0,0,267", true, 12164312, new List<HexPatch>() {
                new HexPatch(
                    0x1972B0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19943B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FF2E9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FF2F2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33F03C,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_267.dll", "34,0,0,267", true, 13154520, new List<HexPatch>() {
                new HexPatch(
                    0x197918,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199B37,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x317C9F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x317CA8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3595B4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_267.dll", "34,0,0,267", false, 9903832, new List<HexPatch>() {
                new HexPatch(
                    0x136747,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x138020,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2658A1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2658AA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x298C4A,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_267.dll", "34,0,0,267", false, 10725080, new List<HexPatch>() {
                new HexPatch(
                    0x135539,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136E21,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x277E10,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x277E19,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AC740,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_267.ocx", "34,0,0,267", true, 12609752, new List<HexPatch>() {
                new HexPatch(
                    0x196CA0,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198ECB,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FFEC9,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FFED2,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33F9B4,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_267.ocx", "34,0,0,267", true, 13849816, new List<HexPatch>() {
                new HexPatch(
                    0x80A28,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82C09,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1628D8,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x249160,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x249169,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_267.ocx", "34,0,0,267", true, 13587160, new List<HexPatch>() {
                new HexPatch(
                    0x196CFC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198F1B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x31755F,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x317568,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x358C5C,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_267.ocx", "34,0,0,267", true, 14690520, new List<HexPatch>() {
                new HexPatch(
                    0x161418,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24DB61,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DB6A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_267.ocx", "34,0,0,267", false, 9976024, new List<HexPatch>() {
                new HexPatch(
                    0x136041,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x13791A,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2642C1,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2642CA,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x297321,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_267.ocx", "34,0,0,267", false, 11890392, new List<HexPatch>() {
                new HexPatch(
                    0x7A2B0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C579,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1627E0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23D30B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23D314,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_267.ocx", "34,0,0,267", false, 10792664, new List<HexPatch>() {
                new HexPatch(
                    0x134DEA,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1366C3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x276560,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x276569,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AA9B6,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_267.ocx", "34,0,0,267", false, 12768984, new List<HexPatch>() {
                new HexPatch(
                    0x7A910,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CC29,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x176820,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25937B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x259384,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,267", true, 15305440, new List<HexPatch>() {
                new HexPatch(
                    0x56CB19,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB56,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56CB5F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56CB64,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5CAE40,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,267", true, 16792304, new List<HexPatch>() {
                new HexPatch(
                    0x3CA1D9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA216,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA21F,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA224,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B45C0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,267", false, 10734296, new List<HexPatch>() {
                new HexPatch(
                    0x62D14,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62D1D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1DA1BC,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,267", false, 11553496, new List<HexPatch>() {
                new HexPatch(
                    0x187B83,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x187B8C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326E13,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_277.dll", "34,0,0,277", true, 16092128, new List<HexPatch>() {
                new HexPatch(
                    0x217269,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x217272,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x418F80,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x426040,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4287D5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_277.dll", "34,0,0,277", true, 17040352, new List<HexPatch>() {
                new HexPatch(
                    0x328A21,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328A2A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x43AC30,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x450470,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x452BF5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_277.dll", "34,0,0,277", false, 8990688, new List<HexPatch>() {
                new HexPatch(
                    0x13F82F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F838,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25950F,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x261096,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26109B,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2628F6,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_277.dll", "34,0,0,277", false, 9595872, new List<HexPatch>() {
                new HexPatch(
                    0x198D35,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198D3E,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25C206,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268F73,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x268F78,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26A7E6,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_277.dll", "34,0,0,277", true, 12165088, new List<HexPatch>() {
                new HexPatch(
                    0x197378,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1995A3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FF029,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FF032,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33ED10,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_277.dll", "34,0,0,277", true, 13154784, new List<HexPatch>() {
                new HexPatch(
                    0x197CEC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199E6B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x317BCF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x317BD8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x35950C,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_277.dll", "34,0,0,277", false, 9904608, new List<HexPatch>() {
                new HexPatch(
                    0x13684F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x138143,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x265891,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26589A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x298C51,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_277.dll", "34,0,0,277", false, 10725344, new List<HexPatch>() {
                new HexPatch(
                    0x135579,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136E8B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x277FC0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x277FC9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AC82C,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_277.ocx", "34,0,0,277", true, 12609504, new List<HexPatch>() {
                new HexPatch(
                    0x197024,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19924F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x300019,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x300022,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33FA74,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_277.ocx", "34,0,0,277", true, 13850080, new List<HexPatch>() {
                new HexPatch(
                    0x80A98,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82C79,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x162B38,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x248FA0,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x248FA9,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_277.ocx", "34,0,0,277", true, 13586912, new List<HexPatch>() {
                new HexPatch(
                    0x196C94,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198EB3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3172BF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3172C8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3587AC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                // This version has the time bomb enabled mistakenly by default
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_277.ocx", "34,0,0,277", true, 14691296, new List<HexPatch>() {
                new HexPatch(
                    0x8E0EC,
                    new byte[] { 0x40, 0x53, 0x55, 0x56, 0x57, 0x41 },
                    new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x8E1E8,
                    new byte[] { 0x40, 0x53, 0x55, 0x56, 0x57, 0x41 },
                    new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x161428,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x16470F,
                    new byte[] { 0xEB, 0x02 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DB71,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DB7A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_277.ocx", "34,0,0,277", false, 9976800, new List<HexPatch>() {
                new HexPatch(
                    0x13604D,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137932,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x264011,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26401A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x29700F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_277.ocx", "34,0,0,277", false, 11890656, new List<HexPatch>() {
                new HexPatch(
                    0x7A330,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C649,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x162D80,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23D8FB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23D904,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_277.ocx", "34,0,0,277", false, 10792928, new List<HexPatch>() {
                new HexPatch(
                    0x134F69,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1368AD,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x276520,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x276529,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AA834,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_277.ocx", "34,0,0,277", false, 12769248, new List<HexPatch>() {
                new HexPatch(
                    0x7A8F0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7CBB9,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1769A0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25930B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x259314,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: PepperFlashPlayer
                "Chinese Mac 64-bit Chrome Plugin (Pepper)", "34,0,0,277", true, 15301296, new List<HexPatch>() {
                new HexPatch(
                    0x56BB19,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BB56,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x56BB5F,
                    new byte[] { 0x0F, 0x84, 0x30, 0x03 },
                    new byte[] { 0xE9, 0x31, 0x03, 0x00 }
                ),
                new HexPatch(
                    0x56BB64,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x5C9E40,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer-10.6
                "Chinese Mac 64-bit Firefox Plugin (NPAPI)", "34,0,0,277", true, 16792304, new List<HexPatch>() {
                new HexPatch(
                    0x3CA1D9,
                    new byte[] { 0x74, 0x4A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA216,
                    new byte[] { 0x74, 0x0D },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3CA21F,
                    new byte[] { 0x0F, 0x84, 0xED, 0x02 },
                    new byte[] { 0xE9, 0xEE, 0x02, 0x00 }
                ),
                new HexPatch(
                    0x3CA224,
                    new byte[] { 0x00 },
                    new byte[] { 0x90 }
                ),
                new HexPatch(
                    0x4B45C0,
                    new byte[] { 0x55, 0x48, 0x89, 0xE5, 0x41, 0x56 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa.exe
                "Chinese Standalone Flash Player", "34,0,0,277", false, 10735072, new List<HexPatch>() {
                new HexPatch(
                    0x62D14,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x62D1D,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x1DA1BC,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: flashplayer_sa_debug.exe
                "Chinese Standalone Flash Player", "34,0,0,277", false, 11554784, new List<HexPatch>() {
                new HexPatch(
                    0x187B83,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x187B8C,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x326E13,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_282.dll", "34,0,0,282", true, 16092128, new List<HexPatch>() {
                new HexPatch(
                    0x217269,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x217272,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x418F80,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x426040,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x4287D5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 64-bit Plugin (Pepper)", "pepflashplayer64_34_0_0_282.dll", "34,0,0,282", true, 17040352, new List<HexPatch>() {
                new HexPatch(
                    0x328A21,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x328A2A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x43AC30,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x10, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x450470,
                    new byte[] { 0x40, 0x55, 0x57, 0x41, 0x54, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x452BF5,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_282.dll", "34,0,0,282", false, 8990688, new List<HexPatch>() {
                new HexPatch(
                    0x13F82F,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x13F838,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25950F,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x261096,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x26109B,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x2628F6,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Chrome 32-bit Plugin (Pepper)", "pepflashplayer32_34_0_0_282.dll", "34,0,0,282", false, 9595360, new List<HexPatch>() {
                new HexPatch(
                    0x198CE5,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x198CEE,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x25C196,
                    new byte[] { 0x53, 0x56, 0x8B, 0xF1, 0x57, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x268F03,
                    new byte[] { 0x81, 0xEC, 0x20, 0x01 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00 }
                ),
                new HexPatch(
                    0x268F08,
                    new byte[] { 0x00 },
                    new byte[] { 0xC3 }
                ),
                new HexPatch(
                    0x26A776,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_282.dll", "34,0,0,282", true, 12165088, new List<HexPatch>() {
                new HexPatch(
                    0x197368,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199593,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2FF019,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2FF022,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33ED00,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 64-bit Plugin (NPAPI)", "NPSWF64_34_0_0_282.dll", "34,0,0,282", true, 13154784, new List<HexPatch>() {
                new HexPatch(
                    0x197CEC,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x199E6B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x317BCF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x317BD8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x35950C,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_282.dll", "34,0,0,282", false, 9904608, new List<HexPatch>() {
                new HexPatch(
                    0x13684F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x138143,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x265891,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26589A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x298C51,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese Firefox 32-bit Plugin (NPAPI)", "NPSWF32_34_0_0_282.dll", "34,0,0,282", false, 10725344, new List<HexPatch>() {
                new HexPatch(
                    0x135579,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x136E8B,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x277FC0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x277FC9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AC82C,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_282.ocx", "34,0,0,282", true, 12609504, new List<HexPatch>() {
                new HexPatch(
                    0x197024,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x19924F,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x300019,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x300022,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x33FA74,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_282.ocx", "34,0,0,282", true, 13850080, new List<HexPatch>() {
                new HexPatch(
                    0x80A98,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x82C79,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x162B38,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x248FA0,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x248FA9,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_282.ocx", "34,0,0,282", true, 13586912, new List<HexPatch>() {
                new HexPatch(
                    0x196C94,
                    new byte[] { 0x48, 0x8B, 0xC4, 0x48, 0x89, 0x58 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x198EB3,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x3172BF,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3172C8,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x3587AC,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x48 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 64-bit Plugin (ActiveX)", "Flash64_34_0_0_282.ocx", "34,0,0,282", true, 14691296, new List<HexPatch>() {
                new HexPatch(
                    0x161428,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x24DB71,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x24DB7A,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_282.ocx", "34,0,0,282", false, 9976800, new List<HexPatch>() {
                new HexPatch(
                    0x13604D,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x137932,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x264011,
                    new byte[] { 0x74, 0x23 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x26401A,
                    new byte[] { 0x75, 0x1A },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x29700F,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_282.ocx", "34,0,0,282", false, 11890656, new List<HexPatch>() {
                new HexPatch(
                    0x7A330,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x7C649,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x162D80,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x23D8FB,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x23D904,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_282.ocx", "34,0,0,282", false, 10792928, new List<HexPatch>() {
                new HexPatch(
                    0x134FAB,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x28 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x1368EF,
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x2764F0,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2764F9,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x2AA7F4,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x53, 0x56, 0x8B },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
            }),
            new PatchableBinary(
                // Debug version
                // Windows 8+ specific plugin
                "Chinese IE 32-bit Plugin (ActiveX)", "Flash32_34_0_0_282.ocx", "34,0,0,282", false, 12769248, new List<HexPatch>() {
                new HexPatch(
                    0x7A8F0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x81, 0xEC, 0x20 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    new byte[] { 0x75 },
                    new byte[] { 0xEB }
                ),
                new HexPatch(
                    0x1769A0,
                    new byte[] { 0x55, 0x8B, 0xEC, 0x56, 0x8B, 0xF1 },
                    new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 }
                ),
                new HexPatch(
                    0x25930B,
                    new byte[] { 0x74, 0x21 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x259314,
                    new byte[] { 0x75, 0x18 },
                    new byte[] { 0x90, 0x90 }
                ),
            }),
            new PatchableBinary(
                // WARNING: This binary can only be applied using the "Patch File..." option
                // Filename: FlashPlayer.exe
                "Adobe Animate Flash Player", "35,0,0,0", true, 13168408, new List<HexPatch>() {
                new HexPatch(
                    0x7531A,
                    new byte[] { 0x74, 0x28 },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x75323,
                    new byte[] { 0x75, 0x1F },
                    new byte[] { 0x90, 0x90 }
                ),
                new HexPatch(
                    0x251B84,
                    new byte[] { 0x48, 0x89, 0x5C, 0x24, 0x08, 0x57 },
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
                // WARNING: A custom patch is not available for this version.
                // FlashPatch will attempt to apply the generic Flash Player patch.
                "IE 64-bit Plugin (ActiveX)", "Flash64_32_0_0_465.ocx", "32,0,0,465", true, -1, new List<HexPatch>() {
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
