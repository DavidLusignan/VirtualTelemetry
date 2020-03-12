using CoreService.UDPProjectCars2.PacketParser.Data;
using CoreService.UDPProjectCars2.RawPacketHandler;
using System;
using System.Linq;

namespace CoreService.UDPProjectCars2.PacketParser {
    public class PC2PacketFactory {
        private byte _apiVersion { get; }
        public PC2PacketFactory(byte apiVersion) {
            _apiVersion = apiVersion;
        }

        public PC2BasePacket Create(PC2RawPacket rawPacket) {
            switch(_apiVersion) {
                case 8:
                    return CreateVersion8(rawPacket);
                default:
                    throw new NotImplementedException("Project Cars 2 API version not supported");
            }
        }

        public PC2BasePacket CreateVersion8(PC2RawPacket rawPacket) {
            var meta = PC2PacketMeta.Create(rawPacket);
            switch (meta.packetType) {
                case PC2PacketType.Telemetry:
                    return PCars2TelemetryData.Create(rawPacket, meta);
                case PC2PacketType.Timings:
                    return PCars2Timings.Create(rawPacket, meta);
                case PC2PacketType.TimeStats:
                    return PCars2TimeStatsData.Create(rawPacket, meta);
                case PC2PacketType.GameState:
                    return PC2GameStatePacket.Create(rawPacket, meta);
                case PC2PacketType.RaceDefinition:
                    return PC2RaceDefinition.Create(rawPacket, meta);
                case PC2PacketType.Participants:
                case PC2PacketType.WeatherState:
                case PC2PacketType.VehicleNames:
                case PC2PacketType.ParticipantVehicleNames:
                default:
                    throw new NotImplementedException("Project Cars 2 packet type not handled: " + meta.packetType);
            }
        }
    }
}
