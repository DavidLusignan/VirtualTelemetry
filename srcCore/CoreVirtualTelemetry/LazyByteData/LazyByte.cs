
namespace Global.LazyByteData {
    public class LazyByte {
        private byte[] _inputBytes;
        private byte? backingValue = null;
        private int startPosition;

        public LazyByte(byte[] inputBytes, int startPosition) {
            _inputBytes = inputBytes;
            this.startPosition = startPosition;
        }

        public byte Value {
            get {
                if(!backingValue.HasValue) {
                    backingValue = ParseByte(startPosition, _inputBytes);
                }
                return backingValue.Value;
            }
        }

        public static byte ParseByte(int start, byte[] inputBytes) {
            return inputBytes[start];
        }
    }
}
