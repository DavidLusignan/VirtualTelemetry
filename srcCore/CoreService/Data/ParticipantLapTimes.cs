using CoreService.Data;
using CoreService.Storage;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using Global.Enumerable;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    public class ParticipantLapTimes {
        public Key key;
        public int participantIndex;
        public IDictionary<int, ParticipantLapTime> lapTimes;
        public ParticipantLapTimes(Key key, int participantIndex, IDictionary<int, ParticipantLapTime> lapTimes) {
            this.key = key;
            this.participantIndex = participantIndex;
            this.lapTimes = lapTimes;
        }

        internal ParticipantLapTimesDTO DTO() {
            return new ParticipantLapTimesDTO(key, participantIndex, lapTimes);
        }
    }
}
