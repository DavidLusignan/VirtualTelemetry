namespace CoreService.UDPProjectCars2.PacketParser {
    public abstract class PC2BasePacket {
        public PC2PacketMeta baseUDP { get; }
        public PC2BasePacket(PC2PacketMeta baseUDP) {
            this.baseUDP = baseUDP;
        }
    }
}
