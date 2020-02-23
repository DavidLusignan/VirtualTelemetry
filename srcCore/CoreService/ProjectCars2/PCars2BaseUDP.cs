using PcarsUDP;

namespace CoreService.ProjectCars2 {
    public class PCars2BaseUDP {
        public const int PACKET_LENGTH = 12;
        public uint packetNumber;
        public uint categoryPacketNumber;
        public byte partialPacketIndex;
        public byte partialPacketNumber;
        public PCars2_UDP.PacketType packetType;
        public byte packetVersion;


        public PCars2BaseUDP(uint packetNumber, uint categoryPacketNumber, byte partialPacketIndex, byte partialPacketNumber, byte packetType, byte packetVersion) {
            this.packetNumber = packetNumber;
            this.categoryPacketNumber = categoryPacketNumber;
            this.partialPacketIndex = partialPacketIndex;
            this.partialPacketNumber = partialPacketNumber;
            this.packetType = (PCars2_UDP.PacketType)packetType;
            this.packetVersion = packetVersion;
        }
    }
}
