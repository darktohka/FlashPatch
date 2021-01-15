using System.Collections.Generic;
using System.IO;

namespace FlashPatch {
    public class PatchableBinary {

        private string name;
        private string fileName;
        private string version;
        private bool x64;
        private long fileSize;
        private List<HexPatch> patches;

        public PatchableBinary(string name, string fileName, string version, bool x64, long fileSize, List<HexPatch> patches) {
            this.name = name;
            this.fileName = fileName;
            this.version = version;
            this.x64 = x64;
            this.fileSize = fileSize;
            this.patches = patches;
        }

        public string GetName() {
            return name;
        }

        public string GetFileName() {
            return fileName;
        }

        public string GetVersion() {
            return version;
        }

        public bool IsX64() {
            return x64;
        }

        public long GetFileSize() {
            return fileSize;
        }

        public List<HexPatch> GetPatches() {
            return patches;
        }

        public string GetBackupFileName() {
            return string.Format("{0}.bak_{1}", fileName, x64 ? "x64" : "x86");
        }

        public bool IsPatchable(FileStream file) {
            foreach (HexPatch patch in patches) {
                if (patch.IsPatchable(file)) {
                    return true;
                }
            }

            return false;
        }

        public void PatchFile(FileStream file) {
            foreach (HexPatch patch in patches) {
                if (patch.IsPatchable(file)) {
                    patch.PatchFile(file);
                }
            }
        }
    }
}
