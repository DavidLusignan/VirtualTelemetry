using System.Collections.Generic;


namespace PcarsUDP {
    partial class PCars2_UDP {
        public struct PCars2TimeStatsData {
            public PCars2BaseUDP baseUDP;
            public const int MAX_PARTICIPANTS = 32;
            public uint participantChangedTimestamp;
            public IEnumerable<PCars2ParticipantStatsInfo> participantStats;
        }
    }
}
