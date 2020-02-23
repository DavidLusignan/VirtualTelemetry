using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;


namespace PcarsUDP {
    partial class PCars2_UDP {

        private UdpClient _listener;
        private IPEndPoint _groupEP;


        public PCars2_UDP(UdpClient listen, IPEndPoint group) {
            _listener = listen;
            _groupEP = group;
        }

        public void readPackets() {
            byte[] UDPpacket = _listener.Receive(ref _groupEP);
            Stream stream = new MemoryStream(UDPpacket);
            var binaryReader = new BinaryReader(stream);

            var baseUDP = ReadBaseUDP(stream, binaryReader);
            if (baseUDP.packetType == 0) {
                ReadTelemetryData(stream, binaryReader, baseUDP);
            } else if (baseUDP.packetType == 3) {
                ReadTimings(stream, binaryReader, baseUDP);
            } else if (baseUDP.packetType == 7) {
                ReadTimeStats(stream, binaryReader, baseUDP);
            }
        }

        public PCars2BaseUDP ReadBaseUDP(Stream stream, BinaryReader binaryReader) {
            stream.Position = 0;
            var packetNumber = binaryReader.ReadUInt32();
            var categoryPacketNumber = binaryReader.ReadUInt32();
            var partialPacketIndex = binaryReader.ReadByte();
            var partialPacketNumber = binaryReader.ReadByte();
            var packetType = binaryReader.ReadByte();
            var packetVersion = binaryReader.ReadByte();

            return new PCars2BaseUDP(packetNumber, categoryPacketNumber, partialPacketIndex, partialPacketNumber, packetType, packetVersion);
        }

        public PCars2TelemetryData ReadTelemetryData(Stream stream, BinaryReader binaryReader, PCars2BaseUDP baseUDP) {
            stream.Position = PCars2BaseUDP.PACKET_LENGTH;
            var telemetryData = new PCars2TelemetryData();
            telemetryData.baseUDP = baseUDP;
            telemetryData.viewedParticipantIndex = binaryReader.ReadSByte();
            telemetryData.unfilteredThrottle = binaryReader.ReadByte();
            telemetryData.unfilteredBrake = binaryReader.ReadByte();
            telemetryData.unfilteredSteering = binaryReader.ReadSByte();
            telemetryData.unfilteredClutch = binaryReader.ReadByte();
            telemetryData.carFlags = binaryReader.ReadByte();
            telemetryData.oilTempCelsius = binaryReader.ReadInt16();
            telemetryData.oilPressureKPa = binaryReader.ReadUInt16();
            telemetryData.waterTempCelsius = binaryReader.ReadInt16();
            telemetryData.waterPressureKPa = binaryReader.ReadUInt16();
            telemetryData.fuelPressureKPa = binaryReader.ReadUInt16();
            telemetryData.fuelCapacity = binaryReader.ReadByte();
            telemetryData.brake = binaryReader.ReadByte();
            telemetryData.throttle = binaryReader.ReadByte();
            telemetryData.clutch = binaryReader.ReadByte();
            telemetryData.fuelLevel = binaryReader.ReadSingle();
            telemetryData.speed = binaryReader.ReadSingle();
            telemetryData.rpm = binaryReader.ReadUInt16();
            telemetryData.maxRpm = binaryReader.ReadUInt16();
            telemetryData.steering = binaryReader.ReadSByte();
            telemetryData.gearNumGears = binaryReader.ReadByte();
            telemetryData.boostAmount = binaryReader.ReadByte();
            telemetryData.crashState = binaryReader.ReadByte();
            telemetryData.odometerKM = binaryReader.ReadSingle();
            telemetryData.orientationX = binaryReader.ReadSingle();
            telemetryData.orientationY = binaryReader.ReadSingle();
            telemetryData.orientationZ = binaryReader.ReadSingle();
            telemetryData.localVelocityX = binaryReader.ReadSingle();
            telemetryData.localVelocityY = binaryReader.ReadSingle();
            telemetryData.localVelocityZ = binaryReader.ReadSingle();
            telemetryData.worldVelocityX = binaryReader.ReadSingle();
            telemetryData.worldVelocityY = binaryReader.ReadSingle();
            telemetryData.worldVelocityZ = binaryReader.ReadSingle();
            telemetryData.angularVelocityX = binaryReader.ReadSingle();
            telemetryData.angularVelocityY = binaryReader.ReadSingle();
            telemetryData.angularVelocityZ = binaryReader.ReadSingle();
            telemetryData.localAccelerationX = binaryReader.ReadSingle();
            telemetryData.localAccelerationY = binaryReader.ReadSingle();
            telemetryData.localAccelerationZ = binaryReader.ReadSingle();
            telemetryData.worldAccelerationX = binaryReader.ReadSingle();
            telemetryData.worldAccelerationY = binaryReader.ReadSingle();
            telemetryData.worldAccelerationZ = binaryReader.ReadSingle();
            telemetryData.extentsCentreX = binaryReader.ReadSingle();
            telemetryData.extentsCentreY = binaryReader.ReadSingle();
            telemetryData.extentsCentreZ = binaryReader.ReadSingle();
            telemetryData.tyreFlagsFL = binaryReader.ReadByte();
            telemetryData.tyreFlagsFR = binaryReader.ReadByte();
            telemetryData.tyreFlagsRL = binaryReader.ReadByte();
            telemetryData.tyreFlagsRR = binaryReader.ReadByte();
            telemetryData.terrainFL = binaryReader.ReadByte();
            telemetryData.terrainFR = binaryReader.ReadByte();
            telemetryData.terrainRL = binaryReader.ReadByte();
            telemetryData.terrainRR = binaryReader.ReadByte();
            telemetryData.tyreYFL = binaryReader.ReadSingle();
            telemetryData.tyreYFR = binaryReader.ReadSingle();
            telemetryData.tyreYRL = binaryReader.ReadSingle();
            telemetryData.tyreYRR = binaryReader.ReadSingle();
            telemetryData.tyreRPSFL = binaryReader.ReadSingle();
            telemetryData.tyreRPSFR = binaryReader.ReadSingle();
            telemetryData.tyreRPSRL = binaryReader.ReadSingle();
            telemetryData.tyreRPSRR = binaryReader.ReadSingle();
            telemetryData.tyreTempFL = binaryReader.ReadByte();
            telemetryData.tyreTempFR = binaryReader.ReadByte();
            telemetryData.tyreTempRL = binaryReader.ReadByte();
            telemetryData.tyreTempRR = binaryReader.ReadByte();
            telemetryData.tyreHeightAboveGroundFL = binaryReader.ReadSingle();
            telemetryData.tyreHeightAboveGroundFR = binaryReader.ReadSingle();
            telemetryData.tyreHeightAboveGroundRL = binaryReader.ReadSingle();
            telemetryData.tyreHeightAboveGroundRR = binaryReader.ReadSingle();
            telemetryData.tyreWearFL = binaryReader.ReadByte();
            telemetryData.tyreWearFR = binaryReader.ReadByte();
            telemetryData.tyreWearRL = binaryReader.ReadByte();
            telemetryData.tyreWearRR = binaryReader.ReadByte();
            telemetryData.brakeDamageFL = binaryReader.ReadByte();
            telemetryData.brakeDamageFR = binaryReader.ReadByte();
            telemetryData.brakeDamageRL = binaryReader.ReadByte();
            telemetryData.brakeDamageRR = binaryReader.ReadByte();
            telemetryData.suspensionDamageFL = binaryReader.ReadByte();
            telemetryData.suspensionDamageFR = binaryReader.ReadByte();
            telemetryData.suspensionDamageRL = binaryReader.ReadByte();
            telemetryData.suspensionDamageRR = binaryReader.ReadByte();
            telemetryData.brakeTempCelsiusFL = binaryReader.ReadInt16();
            telemetryData.brakeTempCelsiusFR = binaryReader.ReadInt16();
            telemetryData.brakeTempCelsiusRL = binaryReader.ReadInt16();
            telemetryData.brakeTempCelsiusRR = binaryReader.ReadInt16();
            telemetryData.tyreTreadTempFL = binaryReader.ReadUInt16();
            telemetryData.tyreTreadTempFR = binaryReader.ReadUInt16();
            telemetryData.tyreTreadTempRL = binaryReader.ReadUInt16();
            telemetryData.tyreTreadTempRR = binaryReader.ReadUInt16();
            telemetryData.tyreLayerTempFL = binaryReader.ReadUInt16();
            telemetryData.tyreLayerTempFR = binaryReader.ReadUInt16();
            telemetryData.tyreLayerTempRL = binaryReader.ReadUInt16();
            telemetryData.tyreLayerTempRR = binaryReader.ReadUInt16();
            telemetryData.tyreCarcassTempFL = binaryReader.ReadUInt16();
            telemetryData.tyreCarcassTempFR = binaryReader.ReadUInt16();
            telemetryData.tyreCarcassTempRL = binaryReader.ReadUInt16();
            telemetryData.tyreCarcassTempRR = binaryReader.ReadUInt16();
            telemetryData.tyreRimTempFL = binaryReader.ReadUInt16();
            telemetryData.tyreRimTempFR = binaryReader.ReadUInt16();
            telemetryData.tyreRimTempRL = binaryReader.ReadUInt16();
            telemetryData.tyreRimTempRR = binaryReader.ReadUInt16();
            telemetryData.tyreInternalAirTempFL = binaryReader.ReadUInt16();
            telemetryData.tyreInternalAirTempFR = binaryReader.ReadUInt16();
            telemetryData.tyreInternalAirTempRL = binaryReader.ReadUInt16();
            telemetryData.tyreInternalAirTempRR = binaryReader.ReadUInt16();
            telemetryData.tyreTempLeftFL = binaryReader.ReadUInt16();
            telemetryData.tyreTempLeftFR = binaryReader.ReadUInt16();
            telemetryData.tyreTempLeftRL = binaryReader.ReadUInt16();
            telemetryData.tyreTempLeftRR = binaryReader.ReadUInt16();
            telemetryData.tyreTempCenterFL = binaryReader.ReadUInt16();
            telemetryData.tyreTempCenterFR = binaryReader.ReadUInt16();
            telemetryData.tyreTempCenterRL = binaryReader.ReadUInt16();
            telemetryData.tyreTempCenterRR = binaryReader.ReadUInt16();
            telemetryData.tyreTempRightFL = binaryReader.ReadUInt16();
            telemetryData.tyreTempRightFR = binaryReader.ReadUInt16();
            telemetryData.tyreTempRightRL = binaryReader.ReadUInt16();
            telemetryData.tyreTempRightRR = binaryReader.ReadUInt16();
            telemetryData.wheelLocalPositionYFL = binaryReader.ReadSingle();
            telemetryData.wheelLocalPositionYFR = binaryReader.ReadSingle();
            telemetryData.wheelLocalPositionYRL = binaryReader.ReadSingle();
            telemetryData.wheelLocalPositionYRR = binaryReader.ReadSingle();
            telemetryData.rideHeightFL = binaryReader.ReadSingle();
            telemetryData.rideHeightFR = binaryReader.ReadSingle();
            telemetryData.rideHeightRL = binaryReader.ReadSingle();
            telemetryData.rideHeightRR = binaryReader.ReadSingle();
            telemetryData.suspensionTravelFL = binaryReader.ReadSingle();
            telemetryData.suspensionTravelFR = binaryReader.ReadSingle();
            telemetryData.suspensionTravelRL = binaryReader.ReadSingle();
            telemetryData.suspensionTravelRR = binaryReader.ReadSingle();
            telemetryData.suspensionVelocityFL = binaryReader.ReadSingle();
            telemetryData.suspensionVelocityFR = binaryReader.ReadSingle();
            telemetryData.suspensionVelocityRL = binaryReader.ReadSingle();
            telemetryData.suspensionVelocityRR = binaryReader.ReadSingle();
            telemetryData.suspensionRideHeightFL = binaryReader.ReadUInt16();
            telemetryData.suspensionRideHeightFR = binaryReader.ReadUInt16();
            telemetryData.suspensionRideHeightRL = binaryReader.ReadUInt16();
            telemetryData.suspensionRideHeightRR = binaryReader.ReadUInt16();
            telemetryData.airPressureFL = binaryReader.ReadUInt16();
            telemetryData.airPressureFR = binaryReader.ReadUInt16();
            telemetryData.airPressureRL = binaryReader.ReadUInt16();
            telemetryData.airPressureRR = binaryReader.ReadUInt16();
            telemetryData.engineSpeed = binaryReader.ReadSingle();
            telemetryData.engineTorque = binaryReader.ReadSingle();
            telemetryData.wingsFront = binaryReader.ReadByte();
            telemetryData.wingsRear = binaryReader.ReadByte();
            return telemetryData;
        }

        public PCars2Timings ReadTimings(Stream stream, BinaryReader binaryReader, PCars2BaseUDP baseUDP) {
            stream.Position = PCars2BaseUDP.PACKET_LENGTH;
            var timings = new PCars2Timings();
            timings.baseUDP = baseUDP;
            timings.numberParticipants = binaryReader.ReadSByte();
            timings.participantsChangedTimestamp = binaryReader.ReadUInt32();
            timings.eventTimeRemaining = binaryReader.ReadSingle();
            timings.splitTimeAhead = binaryReader.ReadSingle();
            timings.splitTimeBehind = binaryReader.ReadSingle();
            timings.splitTime = binaryReader.ReadSingle();
            timings.participants = Enumerable.Range(0, PCars2Timings.MAX_PARTICIPANTS).Select(i => {
                var participant = new PCars2ParticipantTiming();
                participant.worldPositionX = binaryReader.ReadInt16();
                participant.worldPositionY = binaryReader.ReadInt16();
                participant.worldPositionZ = binaryReader.ReadInt16();
                participant.orientationX = binaryReader.ReadInt16();
                participant.orientationY = binaryReader.ReadInt16();
                participant.orientationZ = binaryReader.ReadInt16();
                participant.currentLapDistance = binaryReader.ReadUInt16();
                var fullRacePosition = binaryReader.ReadByte();
                participant.active = fullRacePosition >= 128;
                participant.racePosition = fullRacePosition >= 128 ? (byte)(fullRacePosition - 128) : fullRacePosition;
                byte Sector_ALL = binaryReader.ReadByte();
                var Sector_Extracted = Sector_ALL & 0x0F;
                participant.sector = Sector_Extracted + 1;
                participant.highestFlag = binaryReader.ReadByte();
                participant.pitModeSchedule = binaryReader.ReadByte();
                participant.carIndex = binaryReader.ReadUInt16();
                participant.raceState = binaryReader.ReadByte();
                participant.currentLap = binaryReader.ReadByte();
                participant.currentTime = binaryReader.ReadSingle();
                participant.currentSectorTime = binaryReader.ReadSingle();
                return participant;
            });
            return timings;
        }

        public PCars2TimeStatsData ReadTimeStats(Stream stream, BinaryReader binaryReader, PCars2BaseUDP baseUDP) {
            stream.Position = PCars2BaseUDP.PACKET_LENGTH;
            var timeStatsData = new PCars2TimeStatsData();
            timeStatsData.baseUDP = baseUDP;
            timeStatsData.participantChangedTimestamp = binaryReader.ReadUInt32();
            timeStatsData.participantStats = Enumerable.Range(0, PCars2TimeStatsData.MAX_PARTICIPANTS).Select(i => {
                var participantStatsInfo = new PCars2ParticipantStatsInfo();
                participantStatsInfo.fastestLapTime = binaryReader.ReadSingle();
                participantStatsInfo.lastLapTime = binaryReader.ReadSingle();
                participantStatsInfo.lastSectorTime = binaryReader.ReadSingle();
                participantStatsInfo.fastestSector1 = binaryReader.ReadSingle();
                participantStatsInfo.fastestSector2 = binaryReader.ReadSingle();
                participantStatsInfo.fastestSector3 = binaryReader.ReadSingle();
                return participantStatsInfo;
            });
            return timeStatsData;
        }

        public void close_UDP_Connection() {
            _listener.Close();
        }
    }
}
