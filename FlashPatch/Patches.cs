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
