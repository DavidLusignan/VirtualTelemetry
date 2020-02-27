using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Storage.DTOs {
    public class ParticipantLapTimesDTO {
        public Key key { get; private set; }
        public Guid sessionId;
        public int participantIndex;
        public IDictionary<int, ParticipantLapTime> lapTimes;

        public ParticipantLapTimesDTO(Key key, Guid sessionId, int participantIndex, IDictionary<int, ParticipantLapTime> lapTimes) {
            this.key = key;
            this.sessionId = sessionId;
            this.participantIndex = participantIndex;
            this.lapTimes = lapTimes;
        }
    }
}
