using Global.LazyByteData;
using System;

namespace CoreService {
    public class F12018PacketHeader {
        public LazyUShort packetFormat;
        public LazyByte packetVersion;
        public LazyByte packetId;
        public LazyULong sessionUID;
        public LazyFloat sessionTime;
        public LazyUInt frameIdentifier;
        public LazyByte playerCarIndex;

        public F12018PacketHeader(byte[] inputBytes) {
            packetFormat = new LazyUShort(inputBytes, 0);
            packetVersion = new LazyByte(inputBytes, 2);
            packetId = new LazyByte(inputBytes, 3);
            sessionUID = new LazyULong(inputBytes, 4);
            sessionTime = new LazyFloat(inputBytes, 12);
            frameIdentifier = new LazyUInt(inputBytes, 16);
            playerCarIndex = new LazyByte(inputBytes, 20);
        }
    }
}
