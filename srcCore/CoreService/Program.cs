using CoreService.Data;
using CoreService.F12018;
using CoreService.ProjectCars2;
using Global.Enumerable;
using Global.Observable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    class Program {
        static int viewedParticipant = -1;
        static int previousLap => currentLap - 1;
        static int currentLap = -1;
        static IDictionary<int, float> lapTimes = new Dictionary<int, float>();
        static void Main(string[] args) {
            var cache = new LiveDataCache();
            var handler = new PC2PacketHandler();
            handler.Subscribe(new Observer<DataState>(state => {
                if (state.dataType.Equals(DataStateType.ViewedParticipant)) {
                    viewedParticipant = ((ViewedParticipantIndexState)state).viewedParticipantIndex;
                } else if (state.dataType.Equals(DataStateType.CurrentTime)) {
                    var currentTime = (CurrentTimeState)state;
                    if (currentTime.participantIndex == viewedParticipant) {
                        currentLap = currentTime.currentLap;
                    }
                } else if (state.dataType.Equals(DataStateType.Time)) {
                    var time = (TimeState)state;
                    if (time.participantIndex == viewedParticipant) {
                        Console.WriteLine("received time " + time.lastLapTime + " lap time " + time.lastSectorTime + " sector time.");
                        if (previousLap > 0 && !lapTimes.ContainsKey(previousLap)) {
                            lapTimes.Add(previousLap, time.lastLapTime);
                            lapTimes.ToDictionary().ForEach(kv => Console.WriteLine("Lap {0} : {1}", kv.Key, kv.Value));
                        }
                    }
                }
            }));
            handler.Start();
            while(true){

            }
        }
    }

    public class LapTimeCache {
        private object _stateLock = new object();
        public IEnumerable<ParticipantLapTimes> lapTimes { get; private set; }
        public LapTimeCache(IObservable<DataState> packetHandler) {
            packetHandler.Subscribe(new Observer<DataState>(OnState));
        }

        private void OnState(DataState newState) {
            lock(_stateLock) {
                try {
                    if (newState.dataType.Equals(DataStateType.Time)) {
                        var time = (TimeState)newState;
                        var current = lapTimes.First(l => l.participantIndex.Equals(time.participantIndex));
                        var updated = current.InsertNewTime(time);
                        lapTimes = lapTimes.Except(current).Concat(updated);
                    }
                } catch (Exception e) {
                    Console.WriteLine("Error while updating lap times");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }

    public class ParticipantLapTimes {
        public int participantIndex;
        public IDictionary<int, ParticipantLapTime> lapTimes;
        public ParticipantLapTimes(int participantIndex, IDictionary<int, ParticipantLapTime> lapTimes) {
            this.participantIndex = participantIndex;
            this.lapTimes = lapTimes;
        }

        public ParticipantLapTimes InsertNewTime(TimeState time) {
            return this;
        }
    }

    public class ParticipantLapTime {
        public float lapTime;
        public float sector1Time;
        public float sector2Time;
        public float sector3Time;
        public ParticipantLapTime(float lapTime, float sector1Time, float sector2Time, float sector3Time) {
            this.lapTime = lapTime;
            this.sector1Time = sector1Time;
            this.sector2Time = sector2Time;
            this.sector3Time = sector3Time;
        }
    }
}
