using CoreService.F12018;
using System.ComponentModel;

namespace CoreService {
    class F12018PacketFactory {
        public F12018Packet CreatePacket(byte[] bytes) {
            var header = new F12018PacketHeader(bytes);
            var packetType = (F12018PacketType)header.packetId.Value;
            F12018PacketData data;
            switch(packetType) {
                case F12018PacketType.CAR_TELEMETRY:
                    data = new F12018PacketTelemetry(bytes);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
            return new F12018Packet(header, data);
        }
    }
}
