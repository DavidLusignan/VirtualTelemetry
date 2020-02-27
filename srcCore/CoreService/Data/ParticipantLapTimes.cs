﻿using CoreService.Data;
using Global.Enumerable;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    public class ParticipantLapTimes {
        public int participantIndex;
        public IDictionary<int, ParticipantLapTime> lapTimes;
        public ParticipantLapTimes(int participantIndex, IDictionary<int, ParticipantLapTime> lapTimes) {
            this.participantIndex = participantIndex;
            this.lapTimes = lapTimes;
        }

        public ParticipantLapTimes InsertIfNewTime(TimeState time) {
            if (IsOutLap(time)) {
                return this;
            }
            else if (IsFirstSector(time)) {
                var toInsert = new ParticipantLapTime(-1, time.lastSectorTime, -1, -1);
                var newLapTimes = lapTimes.Concat(0, toInsert);
                return new ParticipantLapTimes(participantIndex, newLapTimes);
            }
            else if (IsNewSector1(time)) {
                var toInsert = new ParticipantLapTime(-1, time.lastSectorTime, -1, -1);
                var newLapTimes = lapTimes.Concat(lapTimes.Keys.Max() + 1, toInsert);
                return new ParticipantLapTimes(participantIndex, newLapTimes);
            }
            else if (IsNewSector2(time)) {
                var toUpdate = new ParticipantLapTime(-1, CurrentLap().sector1Time, time.lastSectorTime, -1);
                var newLapTimes = lapTimes.Except(lapTimes.Keys.Max()).Concat(lapTimes.Keys.Max(), toUpdate);
                return new ParticipantLapTimes(participantIndex, newLapTimes);
            } else if (IsNewSector3(time)) {
                var toUpdate = new ParticipantLapTime(time.lastLapTime, CurrentLap().sector1Time, CurrentLap().sector2Time, time.lastSectorTime);
                var newLapTimes = lapTimes.Except(lapTimes.Keys.Max()).Concat(lapTimes.Keys.Max(), toUpdate);
                return new ParticipantLapTimes(participantIndex, newLapTimes);
            } else {
                return this;
            }
        }

        private ParticipantLapTime CurrentLap() {
            return lapTimes[lapTimes.Keys.Max()];
        }

        private bool IsOutLap(TimeState time) {
            return time.lastSectorTime < 0;
        }

        private bool IsFirstSector(TimeState time) {
            return !lapTimes.Any();
        }

        private bool IsNewSector1(TimeState time) {
            return lapTimes[lapTimes.Keys.Max()].IsComplete && lapTimes[lapTimes.Keys.Max()].sector3Time != time.lastSectorTime;
        }

        private bool IsNewSector2(TimeState time) {
            return lapTimes[lapTimes.Keys.Max()].sector1Time > 0 && 
                lapTimes[lapTimes.Keys.Max()].sector2Time <= 0 && 
                lapTimes[lapTimes.Keys.Max()].sector1Time != time.lastSectorTime;
        }

        private bool IsFirstLap() => lapTimes.Count() == 1;

        private bool IsNewSector3(TimeState time) {
            return lapTimes[lapTimes.Keys.Max()].sector2Time > 0 && 
                lapTimes[lapTimes.Keys.Max()].sector3Time <= 0 && 
                lapTimes[lapTimes.Keys.Max()].sector2Time != time.lastSectorTime;
        }
    }
}