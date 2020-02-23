using System.Collections.Generic;


namespace PcarsUDP {
    partial class PCars2_UDP {
        public struct PCars2Timings {
            public PCars2BaseUDP baseUDP;
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
}
