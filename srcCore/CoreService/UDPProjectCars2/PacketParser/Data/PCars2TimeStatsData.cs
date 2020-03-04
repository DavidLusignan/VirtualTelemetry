using System.Collections.Generic;


namespace CoreService.UDPProjectCars2.PacketParser {
    public class PCars2TimeStatsData : PC2BasePacket {
        public const int MAX_PARTICIPANTS = 32;
        public uint participantChangedTimestamp;
        public IEnumerable<PCars2ParticipantStatsInfo> participantStats;
    }
}
