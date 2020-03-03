using CoreService.Data;
using LiteDB;
using System;

namespace CoreService {
    public class ParticipantLapTime {
        public double lapTime { get; set; }
        public double sector1Time { get; set; }
        public double sector2Time { get; set; }
        public double sector3Time { get; set; }

        [BsonIgnore]
        public bool IsComplete => lapTime != -1 && sector1Time != -1 && sector2Time != -1 && sector3Time != -1;

        public ParticipantLapTime() {

        }

        public ParticipantLapTime(double lapTime, double sector1Time, double sector2Time, double sector3Time) {
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

        public override bool Equals(object obj) {
            return obj is ParticipantLapTime time &&
                   lapTime == time.lapTime &&
                   sector1Time == time.sector1Time &&
                   sector2Time == time.sector2Time &&
                   sector3Time == time.sector3Time;
        }

        public override int GetHashCode() {
            return HashCode.Combine(lapTime, sector1Time, sector2Time, sector3Time);
        }
    }
}
