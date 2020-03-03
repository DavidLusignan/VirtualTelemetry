using PcarsUDP;

namespace CoreService.ProjectCars2 {
    public sealed class PC2RawPacketMeta {
        public const int PACKET_LENGTH = 12;
        public uint packetNumber;
        public uint categoryPacketNumber;
        public byte partialPacketIndex;
        public byte partialPacketNumber;
        public PC2RawHandler.PacketType packetType;
        public byte packetVersion;


        public PC2RawPacketMeta(uint packetNumber, uint categoryPacketNumber, byte partialPacketIndex, byte partialPacketNumber, byte packetType, byte packetVersion) {
            this.packetNumber = packetNumber;
            this.categoryPacketNumber = categoryPacketNumber;
            this.partialPacketIndex = partialPacketIndex;
            this.partialPacketNumber = partialPacketNumber;
            this.packetType = (PC2RawHandler.PacketType)packetType;
            this.packetVersion = packetVersion;
        }
    }
}
