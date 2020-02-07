using CoreService.F12018;
using System;

namespace CoreService {
    class F12018PacketFactory {
        public F12018Packet CreatePacket(byte[] bytes) {
            var header = CreateHeader(bytes);
            var dataType = (F12018PacketType)header.packetId.Value;
            var data = CreateData(bytes, dataType);
            return new F12018Packet(header, data);
        }

        private F12018PacketHeader CreateHeader(byte[] bytes) {
            return new F12018PacketHeader(bytes);
        }

        private F12018PacketData CreateData(byte[] bytes, F12018PacketType dataType) {
            var speedRange = new ReadOnlySpan<byte>(bytes, 21, 2);
            var speed = BitConverter.ToUInt16(speedRange);
            var throttle = bytes[23];
            return new F12018PacketTelemetry(speed, throttle);
        }
    }
}
