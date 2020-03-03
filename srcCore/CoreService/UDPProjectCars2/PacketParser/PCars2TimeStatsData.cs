using System.Collections.Generic;


namespace CoreService.ProjectCars2 {
    public class PCars2TimeStatsData : PC2RawPacket {
        public const int MAX_PARTICIPANTS = 32;
        public uint participantChangedTimestamp;
        public IEnumerable<PCars2ParticipantStatsInfo> participantStats;
    }
}
