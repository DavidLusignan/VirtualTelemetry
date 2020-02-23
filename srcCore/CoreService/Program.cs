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
}
