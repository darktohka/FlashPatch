using System.IO;
using System.Linq;

namespace FlashPatch {
    public class HexPatch {

        private int offset;
        private byte[] originalBytes;
        private byte[] patchedBytes;

        public HexPatch(int offset, byte[] originalBytes, byte[] patchedBytes) {
            this.offset = offset;
            this.originalBytes = originalBytes;
            this.patchedBytes = patchedBytes;
        }

        public int GetOffset() {
            return offset;
        }

        public byte[] GetOriginalBytes() {
            return originalBytes;
        }

        public byte[] GetPatchedBytes() {
            return patchedBytes;
        }
        
        public int GetLength() {
            return originalBytes.Length;
        }

        private byte[] ReadBytes(FileStream fileStream) {
            fileStream.Position = offset;

            int length = GetLength();
            byte[] readBytes = new byte[length];

            fileStream.Read(readBytes, 0, length);
            return readBytes;
        }

        public bool IsPatchable(byte[] readBytes) {
            return Enumerable.SequenceEqual(readBytes, originalBytes);
        }

        public bool IsPatched(byte[] readBytes) {
            return Enumerable.SequenceEqual(readBytes, patchedBytes);
        }

        public bool IsPatchable(FileStream fileStream) {
            return IsPatchable(ReadBytes(fileStream));
        }

        public bool IsPatched(FileStream fileStream) {
            return IsPatched(ReadBytes(fileStream));
        }

        public void PatchFile(FileStream fileStream) {
            fileStream.Position = offset;
            fileStream.Write(patchedBytes, 0, GetLength());
        }
    }
}
