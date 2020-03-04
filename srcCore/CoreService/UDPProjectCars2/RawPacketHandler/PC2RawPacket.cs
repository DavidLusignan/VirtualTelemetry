using System;
using System.IO;

namespace CoreService.UDPProjectCars2.RawPacketHandler {
    public sealed class PC2RawPacket {
        public BinaryReader Data { get; }
        public DateTime TimeStamp { get; }

        private PC2RawPacket(Stream stream, DateTime timeStamp) {
            Data = new BinaryReader(stream);
            TimeStamp = timeStamp;
        }

        public static PC2RawPacket Create(Stream stream) {
            return new PC2RawPacket(stream, DateTime.UtcNow);
        }
    }
}
