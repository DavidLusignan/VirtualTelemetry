using System;
using System.Collections.Generic;
using System.Text;

namespace Global.LazyByteData {
    public class LazyUShort {
        private byte[] _inputBytes;
        private ushort? backingValue = null;
        private int startPosition;

        public LazyUShort(byte[] inputBytes, int startPosition) {
            _inputBytes = inputBytes;
            this.startPosition = startPosition;
        }

        public ushort Value {
            get {
                if (!backingValue.HasValue) {
                    backingValue = ParseUShort(startPosition, _inputBytes);
                }
                return backingValue.Value;
            }
        }

        public static ushort ParseUShort(int start, byte[] inputBytes) {
            var packetFormatRange = new ReadOnlySpan<byte>(inputBytes, start, 2);
            return BitConverter.ToUInt16(packetFormatRange);
        }
    }
}
