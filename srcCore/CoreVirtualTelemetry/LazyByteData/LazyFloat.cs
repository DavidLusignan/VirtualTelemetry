using System;

namespace Global.LazyByteData {
    public class LazyFloat {
        private byte[] _inputBytes;
        private float? backingValue = null;
        private int startPosition;

        public LazyFloat(byte[] inputBytes, int startPosition) {
            _inputBytes = inputBytes;
            this.startPosition = startPosition;
        }

        public float Value {
            get {
                if (!backingValue.HasValue) {
                    backingValue = ParseFloat(startPosition, _inputBytes);
                }
                return backingValue.Value;
            }
        }

        public static float ParseFloat(int start, byte[] inputBytes) {
            var packetFormatRange = new ReadOnlySpan<byte>(inputBytes, start, 4);
            return BitConverter.ToSingle(packetFormatRange);
        }
    }
}
