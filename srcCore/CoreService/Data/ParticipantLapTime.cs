using CoreService.Data;

namespace CoreService {
    public class ParticipantLapTime {
        public float lapTime;
        public float sector1Time;
        public float sector2Time;
        public float sector3Time;
        public bool IsComplete => lapTime != -1 && sector1Time != -1 && sector2Time != -1 && sector3Time != -1;
        public ParticipantLapTime(float lapTime, float sector1Time, float sector2Time, float sector3Time) {
            this.lapTime = lapTime;
            this.sector1Time = sector1Time;
            this.sector2Time = sector2Time;
            this.sector3Time = sector3Time;
        }
        public ParticipantLapTime UpdateTime(TimeState time) {
            if (sector2Time == -1) {
                return new ParticipantLapTime(-1, sector1Time, time.lastSectorTime, -1);
            } else {
                return new ParticipantLapTime(time.lastLapTime, sector1Time, sector2Time, time.lastSectorTime);
            }
        }
    }
}
