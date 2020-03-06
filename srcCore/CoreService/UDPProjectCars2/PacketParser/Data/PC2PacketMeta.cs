using CoreService.UDPProjectCars2.RawPacketHandler;

namespace CoreService.UDPProjectCars2.PacketParser {
    public sealed class PC2PacketMeta {
        public const int PACKET_LENGTH = 12;
        public uint packetNumber;
        public uint categoryPacketNumber;
        public byte partialPacketIndex;
        public byte partialPacketNumber;
        public PC2PacketType packetType;
        public byte packetVersion;

        public PC2PacketMeta(uint packetNumber, uint categoryPacketNumber, byte partialPacketIndex, byte partialPacketNumber, byte packetType, byte packetVersion) {
            this.packetNumber = packetNumber;
            this.categoryPacketNumber = categoryPacketNumber;
            this.partialPacketIndex = partialPacketIndex;
            this.partialPacketNumber = partialPacketNumber;
            this.packetType = (PC2PacketType)packetType;
            this.packetVersion = packetVersion;
        }

        public static PC2PacketMeta Create(PC2RawPacket rawPacket) {
            var packetNumber = rawPacket.Data.ReadUInt32();
            var categoryPacketNumber = rawPacket.Data.ReadUInt32();
            var partialPacketIndex = rawPacket.Data.ReadByte();
            var partialPacketNumber = rawPacket.Data.ReadByte();
            var packetType = rawPacket.Data.ReadByte();
            var packetVersion = rawPacket.Data.ReadByte();

            return new PC2PacketMeta(packetNumber, categoryPacketNumber, partialPacketIndex, partialPacketNumber, packetType, packetVersion);
        }
    }
}
