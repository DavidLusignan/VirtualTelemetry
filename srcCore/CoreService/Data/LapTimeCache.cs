using CoreService.Data;
using Global.Enumerable;
using Global.Observable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    public class LapTimeCache {
        private object _stateLock = new object();
        public IEnumerable<ParticipantLapTimes> lapTimes { get; private set; }
        public LapTimeCache(IObservable<DataState> packetHandler) {
            lapTimes = new List<ParticipantLapTimes>();
            packetHandler.Subscribe(new Observer<DataState>(OnState));
        }

        private void OnState(DataState newState) {
            lock(_stateLock) {
                try {
                    if (newState.dataType.Equals(DataStateType.Time)) {
                        var time = (TimeState)newState;
                        ParticipantLapTimes current;
                        if (lapTimes.Any(l => l.participantIndex.Equals(time.participantIndex))) {
                            current = lapTimes.First(l => l.participantIndex.Equals(time.participantIndex));
                        } else {
                            current = new ParticipantLapTimes(time.participantIndex, new Dictionary<int, ParticipantLapTime>());
                        }
                        var updated = current.InsertIfNewTime(time);
                        lapTimes = lapTimes.Except(current).Concat(updated);
                    }
                } catch (Exception e) {
                    Console.WriteLine("Error while updating lap times");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
