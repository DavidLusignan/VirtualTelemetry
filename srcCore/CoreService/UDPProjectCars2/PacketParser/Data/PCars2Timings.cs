using CoreService.UDPProjectCars2.RawPacketHandler;
using System.Collections.Generic;
using System.Linq;

namespace CoreService.UDPProjectCars2.PacketParser {
    public class PCars2Timings : PC2BasePacket {
        public const int MAX_PARTICIPANTS = 32;
        public sbyte numberParticipants;
        public uint participantsChangedTimestamp;
        public float eventTimeRemaining;
        public float splitTimeAhead;
        public float splitTimeBehind;
        public float splitTime;
        public IEnumerable<PCars2ParticipantTiming> participants;

        public static PCars2Timings Create(PC2RawPacket rawPacket, PC2PacketMeta baseUDP) {
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
                participant.raceState = (PC2RaceState)rawPacket.Data.ReadByte();
                participant.currentLap = rawPacket.Data.ReadByte();
                participant.currentTime = rawPacket.Data.ReadSingle();
                participant.currentSectorTime = rawPacket.Data.ReadSingle();
                return participant;
            });
            return timings;
        }
    }
}
