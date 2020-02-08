using Global.LazyByteData;

namespace CoreService.F12018 {
    public class F12018PacketTelemetry : F12018PacketData {
        public LazyUShort speed;
        public LazyByte throttle;

        public F12018PacketTelemetry(byte[] inputBytes) {
            speed = new LazyUShort(inputBytes, 21);
            throttle = new LazyByte(inputBytes, 23);
        }
    }
}
