namespace CoreService {
    public class F12018PacketHeader {
        public ushort packetFormat;
        public byte packetVersion;
        public byte packetId;
        public ulong sessionUID;
        public float sessionTime;
        public uint frameIdentifier;
        public byte playerCarIndex;

        public F12018PacketHeader(ushort packetFormat, byte packetVersion, byte packetId, ulong sessionUID, float sessionTime, uint frameIdentifier, byte playerCarIndex) {
            this.packetFormat = packetFormat;
            this.packetVersion = packetVersion;
            this.packetId = packetId;
            this.sessionUID = sessionUID;
            this.sessionTime = sessionTime;
            this.frameIdentifier = frameIdentifier;
            this.playerCarIndex = playerCarIndex;
        }
    }
}
