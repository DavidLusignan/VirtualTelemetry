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
            //handler.Subscribe(new Observer<DataState>(state => {
            //    if (state.dataType.Equals(DataStateType.ViewedParticipant)) {
            //        viewedParticipant = ((ViewedParticipantIndexState)state).viewedParticipantIndex;
            //    } else if (state.dataType.Equals(DataStateType.CurrentTime)) {
            //        var currentTime = (CurrentTimeState)state;
            //        if (currentTime.participantIndex == viewedParticipant) {
            //            currentLap = currentTime.currentLap;
            //        }
            //    } else if (state.dataType.Equals(DataStateType.Time)) {
            //        var time = (TimeState)state;
            //        if (time.participantIndex == viewedParticipant) {
            //            Console.WriteLine("received time " + time.lastLapTime + " lap time " + time.lastSectorTime + " sector time.");
            //            if (previousLap > 0 && !lapTimes.ContainsKey(previousLap)) {
            //                lapTimes.Add(previousLap, time.lastLapTime);
            //                lapTimes.ToDictionary().ForEach(kv => Console.WriteLine("Lap {0} : {1}", kv.Key, kv.Value));
            //            }
            //        }
            //    }
            //}));
            var lapTimeCache = new LapTimeCache(handler);
            handler.Start();
            while(true){
                Console.WriteLine("Enter to dump");
                Console.ReadLine();
                Console.Clear();
                lapTimeCache.lapTimes.First(lp => lp.participantIndex.Equals(0)).lapTimes.ForEach(lapTime => {
                    Console.WriteLine("Lap " + lapTime.Key + "; Total : " + lapTime.Value.lapTime + "; S1: " + lapTime.Value.sector1Time + "; S2: " + lapTime.Value.sector2Time + "; S3: " + lapTime.Value.sector3Time);
                });
            }
        }
    }

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
            if (!lapTimes.Any()) {
                var toInsert = new ParticipantLapTime(-1, time.lastSectorTime, -1, -1);
                var newLapTimes = lapTimes.Concat(1, toInsert);
                return new ParticipantLapTimes(participantIndex, newLapTimes);
            }
            else if (lapTimes[lapTimes.Keys.Max()].IsComplete) {
                var toInsert = new ParticipantLapTime(-1, time.lastSectorTime, -1, -1);
                var newLapTimes = lapTimes.Concat(lapTimes.Keys.Max() + 1, toInsert);
                return new ParticipantLapTimes(participantIndex, newLapTimes);
            } else {
                var toUpdate = lapTimes[lapTimes.Keys.Max()].UpdateTime(time);
                var newLapTimes = lapTimes.Except(lapTimes.Keys.Max()).Concat(lapTimes.Keys.Max(), toUpdate);
                return new ParticipantLapTimes(participantIndex, newLapTimes);
            }
        }
    }

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
