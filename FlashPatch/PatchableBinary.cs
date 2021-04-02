using System.Collections.Generic;
using System.IO;

namespace FlashPatch {
    public class PatchableBinary {

        private string name;
        private string version;
        private bool x64;
        private long fileSize;
        private List<string> filenames;
        private List<HexPatch> patches;
        private List<string> alternatePaths;

        public PatchableBinary(string name, string version, bool x64, long fileSize, List<string> filenames, List<HexPatch> patches, List<string> alternatePaths) {
            this.name = name;
            this.version = version;
            this.x64 = x64;
            this.fileSize = fileSize;
            this.filenames = filenames;
            this.patches = patches;
            this.alternatePaths = alternatePaths;
        }

        public PatchableBinary(string name, string version, bool x64, long fileSize, List<HexPatch> patches) :
            this(name, version, x64, fileSize, null, patches, new List<string>()) {
            // Empty.
        }

        public PatchableBinary(string name, string version, bool x64, long fileSize, List<string> filenames, List<HexPatch> patches) :
            this(name, version, x64, fileSize, filenames, patches, new List<string>()) {
            // Empty.
        }

        public PatchableBinary(string name, string filename, string version, bool x64, long fileSize, List<HexPatch> patches, List<string> alternatePaths) :
            this(name, version, x64, fileSize, new List<string>() { filename }, patches, alternatePaths) {
            // Empty.
        }

        public PatchableBinary(string name, string filename, string version, bool x64, long fileSize, List<HexPatch> patches) :
            this(name, version, x64, fileSize, new List<string>() { filename }, patches, new List<string>()) {
            // Empty.
        }

        public string GetName() {
            return name;
        }

        public List<string> GetFilenames() {
            return filenames;
        }

        public bool HasFilenames() {
            return filenames != null;
        }

        public string GetVersion() {
            return version;
        }

        public bool HasVersion() {
            return !string.IsNullOrWhiteSpace(version);
        }

        public bool IsX64() {
            return x64;
        }

        public long GetFileSize() {
            return fileSize;
        }

        public bool HasFileSize() {
            return fileSize >= 0;
        }

        public List<HexPatch> GetPatches() {
            return patches;
        }

        public List<string> GetAlternatePaths() {
            return alternatePaths;
        }

        public string GetBackupFileName(string filename) {
            return string.Format("{0}.bak_{1}", filename, x64 ? "x64" : "x86");
        }

        public bool IsPatchable(FileStream file) {
            foreach (HexPatch patch in patches) {
                if (patch.IsPatchable(file)) {
                    return true;
                }
            }

            return false;
        }

        public bool IsPatched(FileStream file) {
            foreach (HexPatch patch in patches) {
                if (patch.IsPatched(file)) {
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
