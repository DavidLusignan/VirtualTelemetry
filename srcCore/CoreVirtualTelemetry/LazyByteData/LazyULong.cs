
using System;

namespace Global.LazyByteData {
    public class LazyULong {
        private byte[] _inputBytes;
        private ulong? backingValue = null;
        private int startPosition;

        public LazyULong(byte[] inputBytes, int startPosition) {
            _inputBytes = inputBytes;
            this.startPosition = startPosition;
        }

        public ulong Value {
            get {
                if (!backingValue.HasValue) {
                    backingValue = ParseULong(startPosition, _inputBytes);
                }
                return backingValue.Value;
            }
        }

        public static ulong ParseULong(int start, byte[] inputBytes) {
            var packetFormatRange = new ReadOnlySpan<byte>(inputBytes, start, 8);
            return BitConverter.ToUInt64(packetFormatRange);
        }
    }
}
