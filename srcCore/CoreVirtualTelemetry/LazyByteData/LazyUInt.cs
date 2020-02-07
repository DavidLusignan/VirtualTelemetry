using System;
using System.Collections.Generic;
using System.Text;

namespace Global.LazyByteData {
    public class LazyUInt {
        private byte[] _inputBytes;
        private uint? backingValue = null;
        private int startPosition;

        public LazyUInt(byte[] inputBytes, int startPosition) {
            _inputBytes = inputBytes;
            this.startPosition = startPosition;
        }

        public uint Value {
            get {
                if (!backingValue.HasValue) {
                    backingValue = ParseUInt(startPosition, _inputBytes);
                }
                return backingValue.Value;
            }
        }

        public static uint ParseUInt(int start, byte[] inputBytes) {
            var packetFormatRange = new ReadOnlySpan<byte>(inputBytes, start, 4);
            return BitConverter.ToUInt32(packetFormatRange);
        }
    }
}
