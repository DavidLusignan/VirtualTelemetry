using CoreService.UDPProjectCars2.RawPacketHandler;
using System.Collections.Generic;
using System.Linq;

namespace CoreService.UDPProjectCars2.PacketParser {
    public class PCars2TimeStatsData : PC2BasePacket {
        public const int MAX_PARTICIPANTS = 32;
        public uint participantChangedTimestamp;
        public IEnumerable<PCars2ParticipantStatsInfo> participantStats;
        public PCars2TimeStatsData(PC2PacketMeta meta) : base(meta) { }
        public static PCars2TimeStatsData Create(PC2RawPacket rawPacket, PC2PacketMeta meta) {
            var timeStatsData = new PCars2TimeStatsData(meta);
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
