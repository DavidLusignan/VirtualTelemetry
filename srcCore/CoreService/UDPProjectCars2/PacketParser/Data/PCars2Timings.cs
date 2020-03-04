using System.Collections.Generic;

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
    }
}
