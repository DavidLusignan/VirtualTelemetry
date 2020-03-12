using CoreService.UDPProjectCars2.RawPacketHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.UDPProjectCars2.PacketParser.Data {
    public class PC2RaceDefinition : PC2BasePacket {
        public float worldFastestLapTime { get; }
        public float personalFastestLapTime { get; }
        public float personalFastestSector1Time { get; }
        public float personalFastestSector2Time { get; }
        public float personalFastestSector3Time { get; }
        public float worldFastestSector1Time { get; }
        public float worldFastestSector2Time { get; }
        public float worldFastestSector3Time { get; }
        public float trackLength { get; }
        public byte[] trackLocation { get; }
        public byte[] trackVariation { get; }
        public byte[] translatedTrackLocation { get; }
        public byte[] translatedTrackVariation { get; }
        public ushort lapsTimeInEvent { get; }
        public sbyte enforcedPitStopLap { get; }

        public PC2RaceDefinition(PC2PacketMeta meta, float worldFastestLapTime, float personalFastestLapTime, float personalFastestSector1Time, float personalFastestSector2Time, float personalFastestSector3Time, float worldFastestSector1Time, float worldFastestSector2Time, float worldFastestSector3Time, float trackLength, byte[] trackLocation, byte[] trackVariation, byte[] translatedTrackLocation, byte[] translatedTrackVariation, ushort lapsTimeInEvent, sbyte enforcedPitStopLap) : base(meta) {
            this.worldFastestLapTime = worldFastestLapTime;
            this.personalFastestLapTime = personalFastestLapTime;
            this.personalFastestSector1Time = personalFastestSector1Time;
            this.personalFastestSector2Time = personalFastestSector2Time;
            this.personalFastestSector3Time = personalFastestSector3Time;
            this.worldFastestSector1Time = worldFastestSector1Time;
            this.worldFastestSector2Time = worldFastestSector2Time;
            this.worldFastestSector3Time = worldFastestSector3Time;
            this.trackLength = trackLength;
            this.trackLocation = trackLocation;
            this.trackVariation = trackVariation;
            this.translatedTrackLocation = translatedTrackLocation;
            this.translatedTrackVariation = translatedTrackVariation;
            this.lapsTimeInEvent = lapsTimeInEvent;
            this.enforcedPitStopLap = enforcedPitStopLap;
        }

        public static PC2RaceDefinition Create(PC2RawPacket rawPacket, PC2PacketMeta meta) {
            var worldFastestLapTime = rawPacket.Data.ReadSingle();
            var personalFastestLapTime = rawPacket.Data.ReadSingle();
            var personalFastestSector1Time = rawPacket.Data.ReadSingle();
            var personalFastestSector2Time = rawPacket.Data.ReadSingle();
            var personalFastestSector3Time = rawPacket.Data.ReadSingle();
            var worldFastestSector1Time = rawPacket.Data.ReadSingle();
            var worldFastestSector2Time = rawPacket.Data.ReadSingle();
            var worldFastestSector3Time = rawPacket.Data.ReadSingle();
            var trackLength = rawPacket.Data.ReadSingle();
            byte[] trackLocation = new byte[64];
            for(int i = 0; i < 64; i++) {
                trackLocation[i] = rawPacket.Data.ReadByte();
            }
            byte[] trackVariation = new byte[64];
            for(int i = 0; i < 64; i++) {
                trackVariation[i] = rawPacket.Data.ReadByte();
            }
            byte[] translatedTrackLocation = new byte[64];
            for(int i = 0; i < 64; i++) {
                translatedTrackLocation[i] = rawPacket.Data.ReadByte();
            }
            byte[] translatedTrackVariation = new byte[64];
            for(int i = 0; i < 64; i++) {
                translatedTrackVariation[i] = rawPacket.Data.ReadByte();
            }
            var lapsTimeInEvent = rawPacket.Data.ReadUInt16();
            var enforcedPitStopLap = rawPacket.Data.ReadSByte();
            return new PC2RaceDefinition(meta, worldFastestLapTime, personalFastestLapTime, personalFastestSector1Time, personalFastestSector2Time, personalFastestSector3Time, worldFastestSector1Time, worldFastestSector2Time, worldFastestSector3Time, trackLength, trackLocation, trackVariation, translatedTrackLocation, translatedTrackVariation, lapsTimeInEvent, enforcedPitStopLap);
        }
    }
}
