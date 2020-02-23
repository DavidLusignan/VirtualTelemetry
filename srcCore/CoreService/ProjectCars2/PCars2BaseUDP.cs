namespace PcarsUDP {
    partial class PCars2_UDP {
        public class PCars2BaseUDP {
            public const int PACKET_LENGTH = 10;
            public uint packetNumber;
            public uint categoryPacketNumber;
            public byte partialPacketIndex;
            public byte partialPacketNumber;
            public byte packetType;
            public byte packetVersion;


            public PCars2BaseUDP(uint packetNumber, uint categoryPacketNumber, byte partialPacketIndex, byte partialPacketNumber, byte packetType, byte packetVersion) {
                this.packetNumber = packetNumber;
                this.categoryPacketNumber = categoryPacketNumber;
                this.partialPacketIndex = partialPacketIndex;
                this.partialPacketNumber = partialPacketNumber;
                this.packetType = packetType;
                this.packetVersion = packetVersion;
            }
        }
    }
}
