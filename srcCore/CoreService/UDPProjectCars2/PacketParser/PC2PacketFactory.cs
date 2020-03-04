using CoreService.UDPProjectCars2.RawPacketHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            var meta = ReadMetaUDP(rawPacket);
            if (meta.packetType == PC2PacketType.Telemetry) {
                return ReadTelemetryData(rawPacket, meta);
            } else if (meta.packetType == PC2PacketType.Timings) {
                return ReadTimings(rawPacket, meta);
            } else if (meta.packetType == PC2PacketType.TimeStats) {
                return ReadTimeStats(rawPacket, meta);
            } else {
                throw new NotImplementedException("Project Cars 2 packet type not handled: " + meta.packetType);
            }
        }

        public PC2PacketMeta ReadMetaUDP(PC2RawPacket rawPacket) {
            var packetNumber = rawPacket.Data.ReadUInt32();
            var categoryPacketNumber = rawPacket.Data.ReadUInt32();
            var partialPacketIndex = rawPacket.Data.ReadByte();
            var partialPacketNumber = rawPacket.Data.ReadByte();
            var packetType = rawPacket.Data.ReadByte();
            var packetVersion = rawPacket.Data.ReadByte();

            return new PC2PacketMeta(packetNumber, categoryPacketNumber, partialPacketIndex, partialPacketNumber, packetType, packetVersion);
        }

        public PCars2TelemetryData ReadTelemetryData(PC2RawPacket rawPacket, PC2PacketMeta meta) {
            var telemetryData = new PCars2TelemetryData();
            telemetryData.baseUDP = meta;
            telemetryData.viewedParticipantIndex = rawPacket.Data.ReadSByte();
            telemetryData.unfilteredThrottle = rawPacket.Data.ReadByte();
            telemetryData.unfilteredBrake = rawPacket.Data.ReadByte();
            telemetryData.unfilteredSteering = rawPacket.Data.ReadSByte();
            telemetryData.unfilteredClutch = rawPacket.Data.ReadByte();
            telemetryData.carFlags = rawPacket.Data.ReadByte();
            telemetryData.oilTempCelsius = rawPacket.Data.ReadInt16();
            telemetryData.oilPressureKPa = rawPacket.Data.ReadUInt16();
            telemetryData.waterTempCelsius = rawPacket.Data.ReadInt16();
            telemetryData.waterPressureKPa = rawPacket.Data.ReadUInt16();
            telemetryData.fuelPressureKPa = rawPacket.Data.ReadUInt16();
            telemetryData.fuelCapacity = rawPacket.Data.ReadByte();
            telemetryData.brake = rawPacket.Data.ReadByte();
            telemetryData.throttle = rawPacket.Data.ReadByte();
            telemetryData.clutch = rawPacket.Data.ReadByte();
            telemetryData.fuelLevel = rawPacket.Data.ReadSingle();
            telemetryData.speed = rawPacket.Data.ReadSingle();
            telemetryData.rpm = rawPacket.Data.ReadUInt16();
            telemetryData.maxRpm = rawPacket.Data.ReadUInt16();
            telemetryData.steering = rawPacket.Data.ReadSByte();
            telemetryData.gearNumGears = rawPacket.Data.ReadByte();
            telemetryData.boostAmount = rawPacket.Data.ReadByte();
            telemetryData.crashState = rawPacket.Data.ReadByte();
            telemetryData.odometerKM = rawPacket.Data.ReadSingle();
            telemetryData.orientationX = rawPacket.Data.ReadSingle();
            telemetryData.orientationY = rawPacket.Data.ReadSingle();
            telemetryData.orientationZ = rawPacket.Data.ReadSingle();
            telemetryData.localVelocityX = rawPacket.Data.ReadSingle();
            telemetryData.localVelocityY = rawPacket.Data.ReadSingle();
            telemetryData.localVelocityZ = rawPacket.Data.ReadSingle();
            telemetryData.worldVelocityX = rawPacket.Data.ReadSingle();
            telemetryData.worldVelocityY = rawPacket.Data.ReadSingle();
            telemetryData.worldVelocityZ = rawPacket.Data.ReadSingle();
            telemetryData.angularVelocityX = rawPacket.Data.ReadSingle();
            telemetryData.angularVelocityY = rawPacket.Data.ReadSingle();
            telemetryData.angularVelocityZ = rawPacket.Data.ReadSingle();
            telemetryData.localAccelerationX = rawPacket.Data.ReadSingle();
            telemetryData.localAccelerationY = rawPacket.Data.ReadSingle();
            telemetryData.localAccelerationZ = rawPacket.Data.ReadSingle();
            telemetryData.worldAccelerationX = rawPacket.Data.ReadSingle();
            telemetryData.worldAccelerationY = rawPacket.Data.ReadSingle();
            telemetryData.worldAccelerationZ = rawPacket.Data.ReadSingle();
            telemetryData.extentsCentreX = rawPacket.Data.ReadSingle();
            telemetryData.extentsCentreY = rawPacket.Data.ReadSingle();
            telemetryData.extentsCentreZ = rawPacket.Data.ReadSingle();
            telemetryData.tyreFlagsFL = rawPacket.Data.ReadByte();
            telemetryData.tyreFlagsFR = rawPacket.Data.ReadByte();
            telemetryData.tyreFlagsRL = rawPacket.Data.ReadByte();
            telemetryData.tyreFlagsRR = rawPacket.Data.ReadByte();
            telemetryData.terrainFL = rawPacket.Data.ReadByte();
            telemetryData.terrainFR = rawPacket.Data.ReadByte();
            telemetryData.terrainRL = rawPacket.Data.ReadByte();
            telemetryData.terrainRR = rawPacket.Data.ReadByte();
            telemetryData.tyreYFL = rawPacket.Data.ReadSingle();
            telemetryData.tyreYFR = rawPacket.Data.ReadSingle();
            telemetryData.tyreYRL = rawPacket.Data.ReadSingle();
            telemetryData.tyreYRR = rawPacket.Data.ReadSingle();
            telemetryData.tyreRPSFL = rawPacket.Data.ReadSingle();
            telemetryData.tyreRPSFR = rawPacket.Data.ReadSingle();
            telemetryData.tyreRPSRL = rawPacket.Data.ReadSingle();
            telemetryData.tyreRPSRR = rawPacket.Data.ReadSingle();
            telemetryData.tyreTempFL = rawPacket.Data.ReadByte();
            telemetryData.tyreTempFR = rawPacket.Data.ReadByte();
            telemetryData.tyreTempRL = rawPacket.Data.ReadByte();
            telemetryData.tyreTempRR = rawPacket.Data.ReadByte();
            telemetryData.tyreHeightAboveGroundFL = rawPacket.Data.ReadSingle();
            telemetryData.tyreHeightAboveGroundFR = rawPacket.Data.ReadSingle();
            telemetryData.tyreHeightAboveGroundRL = rawPacket.Data.ReadSingle();
            telemetryData.tyreHeightAboveGroundRR = rawPacket.Data.ReadSingle();
            telemetryData.tyreWearFL = rawPacket.Data.ReadByte();
            telemetryData.tyreWearFR = rawPacket.Data.ReadByte();
            telemetryData.tyreWearRL = rawPacket.Data.ReadByte();
            telemetryData.tyreWearRR = rawPacket.Data.ReadByte();
            telemetryData.brakeDamageFL = rawPacket.Data.ReadByte();
            telemetryData.brakeDamageFR = rawPacket.Data.ReadByte();
            telemetryData.brakeDamageRL = rawPacket.Data.ReadByte();
            telemetryData.brakeDamageRR = rawPacket.Data.ReadByte();
            telemetryData.suspensionDamageFL = rawPacket.Data.ReadByte();
            telemetryData.suspensionDamageFR = rawPacket.Data.ReadByte();
            telemetryData.suspensionDamageRL = rawPacket.Data.ReadByte();
            telemetryData.suspensionDamageRR = rawPacket.Data.ReadByte();
            telemetryData.brakeTempCelsiusFL = rawPacket.Data.ReadInt16();
            telemetryData.brakeTempCelsiusFR = rawPacket.Data.ReadInt16();
            telemetryData.brakeTempCelsiusRL = rawPacket.Data.ReadInt16();
            telemetryData.brakeTempCelsiusRR = rawPacket.Data.ReadInt16();
            telemetryData.tyreTreadTempFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTreadTempFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTreadTempRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTreadTempRR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreLayerTempFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreLayerTempFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreLayerTempRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreLayerTempRR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreCarcassTempFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreCarcassTempFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreCarcassTempRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreCarcassTempRR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreRimTempFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreRimTempFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreRimTempRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreRimTempRR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreInternalAirTempFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreInternalAirTempFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreInternalAirTempRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreInternalAirTempRR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempLeftFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempLeftFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempLeftRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempLeftRR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempCenterFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempCenterFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempCenterRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempCenterRR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempRightFL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempRightFR = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempRightRL = rawPacket.Data.ReadUInt16();
            telemetryData.tyreTempRightRR = rawPacket.Data.ReadUInt16();
            telemetryData.wheelLocalPositionYFL = rawPacket.Data.ReadSingle();
            telemetryData.wheelLocalPositionYFR = rawPacket.Data.ReadSingle();
            telemetryData.wheelLocalPositionYRL = rawPacket.Data.ReadSingle();
            telemetryData.wheelLocalPositionYRR = rawPacket.Data.ReadSingle();
            telemetryData.rideHeightFL = rawPacket.Data.ReadSingle();
            telemetryData.rideHeightFR = rawPacket.Data.ReadSingle();
            telemetryData.rideHeightRL = rawPacket.Data.ReadSingle();
            telemetryData.rideHeightRR = rawPacket.Data.ReadSingle();
            telemetryData.suspensionTravelFL = rawPacket.Data.ReadSingle();
            telemetryData.suspensionTravelFR = rawPacket.Data.ReadSingle();
            telemetryData.suspensionTravelRL = rawPacket.Data.ReadSingle();
            telemetryData.suspensionTravelRR = rawPacket.Data.ReadSingle();
            telemetryData.suspensionVelocityFL = rawPacket.Data.ReadSingle();
            telemetryData.suspensionVelocityFR = rawPacket.Data.ReadSingle();
            telemetryData.suspensionVelocityRL = rawPacket.Data.ReadSingle();
            telemetryData.suspensionVelocityRR = rawPacket.Data.ReadSingle();
            telemetryData.suspensionRideHeightFL = rawPacket.Data.ReadUInt16();
            telemetryData.suspensionRideHeightFR = rawPacket.Data.ReadUInt16();
            telemetryData.suspensionRideHeightRL = rawPacket.Data.ReadUInt16();
            telemetryData.suspensionRideHeightRR = rawPacket.Data.ReadUInt16();
            telemetryData.airPressureFL = rawPacket.Data.ReadUInt16();
            telemetryData.airPressureFR = rawPacket.Data.ReadUInt16();
            telemetryData.airPressureRL = rawPacket.Data.ReadUInt16();
            telemetryData.airPressureRR = rawPacket.Data.ReadUInt16();
            telemetryData.engineSpeed = rawPacket.Data.ReadSingle();
            telemetryData.engineTorque = rawPacket.Data.ReadSingle();
            telemetryData.wingsFront = rawPacket.Data.ReadByte();
            telemetryData.wingsRear = rawPacket.Data.ReadByte();
            return telemetryData;
        }

        public PCars2Timings ReadTimings(PC2RawPacket rawPacket, PC2PacketMeta baseUDP) {
            var timings = new PCars2Timings();
            timings.baseUDP = baseUDP;
            timings.numberParticipants = rawPacket.Data.ReadSByte();
            timings.participantsChangedTimestamp = rawPacket.Data.ReadUInt32();
            timings.eventTimeRemaining = rawPacket.Data.ReadSingle();
            timings.splitTimeAhead = rawPacket.Data.ReadSingle();
            timings.splitTimeBehind = rawPacket.Data.ReadSingle();
            timings.splitTime = rawPacket.Data.ReadSingle();
            timings.participants = Enumerable.Range(0, PCars2Timings.MAX_PARTICIPANTS).Select(i => {
                var participant = new PCars2ParticipantTiming();
                participant.participantIndex = i;
                participant.worldPositionX = rawPacket.Data.ReadInt16();
                participant.worldPositionY = rawPacket.Data.ReadInt16();
                participant.worldPositionZ = rawPacket.Data.ReadInt16();
                participant.orientationX = rawPacket.Data.ReadInt16();
                participant.orientationY = rawPacket.Data.ReadInt16();
                participant.orientationZ = rawPacket.Data.ReadInt16();
                participant.currentLapDistance = rawPacket.Data.ReadUInt16();
                var fullRacePosition = rawPacket.Data.ReadByte();
                participant.active = fullRacePosition >= 128;
                participant.racePosition = fullRacePosition >= 128 ? (byte)(fullRacePosition - 128) : fullRacePosition;
                byte Sector_ALL = rawPacket.Data.ReadByte();
                var Sector_Extracted = Sector_ALL & 0x0F;
                participant.sector = Sector_Extracted + 1;
                participant.highestFlag = rawPacket.Data.ReadByte();
                participant.pitModeSchedule = rawPacket.Data.ReadByte();
                participant.carIndex = rawPacket.Data.ReadUInt16();
                participant.raceState = rawPacket.Data.ReadByte();
                participant.currentLap = rawPacket.Data.ReadByte();
                participant.currentTime = rawPacket.Data.ReadSingle();
                participant.currentSectorTime = rawPacket.Data.ReadSingle();
                return participant;
            });
            return timings;
        }

        public PCars2TimeStatsData ReadTimeStats(PC2RawPacket rawPacket, PC2PacketMeta baseUDP) {
            var timeStatsData = new PCars2TimeStatsData();
            timeStatsData.baseUDP = baseUDP;
            timeStatsData.participantChangedTimestamp = rawPacket.Data.ReadUInt32();
            timeStatsData.participantStats = Enumerable.Range(0, PCars2TimeStatsData.MAX_PARTICIPANTS).Select(i => {
                var participantStatsInfo = new PCars2ParticipantStatsInfo();
                participantStatsInfo.participantIndex = i;
                participantStatsInfo.fastestLapTime = rawPacket.Data.ReadSingle();
                participantStatsInfo.lastLapTime = rawPacket.Data.ReadSingle();
                participantStatsInfo.lastSectorTime = rawPacket.Data.ReadSingle();
                participantStatsInfo.fastestSector1 = rawPacket.Data.ReadSingle();
                participantStatsInfo.fastestSector2 = rawPacket.Data.ReadSingle();
                participantStatsInfo.fastestSector3 = rawPacket.Data.ReadSingle();
                return participantStatsInfo;
            });
            return timeStatsData;
        }
    }
}
