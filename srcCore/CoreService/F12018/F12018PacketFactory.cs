using System;

namespace CoreService {
    class F12018PacketFactory {
        public F12018Packet CreatePacket(byte[] bytes) {
            return new F12018Packet(CreateHeader(bytes));
        }

        private F12018PacketHeader CreateHeader(byte[] bytes) {
            var packetFormatRange = new ReadOnlySpan<byte>(bytes, 0, 2);
            var packetFormat = BitConverter.ToUInt16(packetFormatRange);
            var packetVersion = bytes[2];
            var packetId = bytes[3];
            var sessionUIDRange = new ReadOnlySpan<byte>(bytes, 4, 8);
            var sessionUID = BitConverter.ToUInt64(sessionUIDRange);
            var sessionTimeRange = new ReadOnlySpan<byte>(bytes, 12, 4);
            var sessionTime = BitConverter.ToSingle(sessionTimeRange);
            var frameIdentifierRange = new ReadOnlySpan<byte>(bytes, 16, 4);
            var frameIdentifier = BitConverter.ToUInt32(frameIdentifierRange);
            var playerCarIndex = bytes[20];

            return new F12018PacketHeader(packetFormat, packetVersion, packetId, sessionUID, sessionTime, frameIdentifier, playerCarIndex);
        }
    }
}
