using System.IO;
using System.Linq;

namespace FlashPatch {
    public class HexPatch {

        private int offset;
        private byte[] originalBytes;
        private byte[] patchedBytes;

        private int offsetFound = -1;

        public HexPatch(int offset, byte[] originalBytes, byte[] patchedBytes) {
            this.offset = offset;
            this.originalBytes = originalBytes;
            this.patchedBytes = patchedBytes;
        }

        public int GetOffset() {
            return offset;
        }

        public bool HasOffset() {
            return offset != -1;
        }

        public int GetFinalOffset() {
            return HasOffset() ? offset : offsetFound;
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

        private int FindBytes(FileStream fileStream, byte[] bytes) {
            fileStream.Position = 0;
            int bufferSize = 65536;
            byte[] buffer = new byte[bufferSize];

            int len = bytes.Length;
            int totalBytes = 0;
            int bytesRead, k;

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0) {
                int limit = bytesRead - len;

                for (int i = 0; i <= limit; ++i) {
                    for (k = 0; k < len; k++) {
                        if (bytes[k] != buffer[i + k]) {
                            break;
                        }
                    }

                    if (k == len) {
                        return totalBytes + i;
                    }
                }

                totalBytes += bytesRead;
            }

            return -1;
        }

        private byte[] ReadBytes(FileStream fileStream) {
            fileStream.Position = GetFinalOffset();

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
            if (!HasOffset()) {
                offsetFound = FindBytes(fileStream, originalBytes);
                return offsetFound != -1;
            }

            return IsPatchable(ReadBytes(fileStream));
        }

        public bool IsPatched(FileStream fileStream) {
            if (!HasOffset()) {
                return FindBytes(fileStream, patchedBytes) != -1;
            }

            return IsPatched(ReadBytes(fileStream));
        }

        public void PatchFile(FileStream fileStream) {
            fileStream.Position = GetFinalOffset();
            fileStream.Write(patchedBytes, 0, GetLength());
        }
    }
}
