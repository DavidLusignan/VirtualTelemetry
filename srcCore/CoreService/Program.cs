using CoreService.F12018;
using CoreService.ProjectCars2;
using Global.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    class Program {
        static int currentLap = -1;
        static IDictionary<int, float> lapTimes = new Dictionary<int, float>();
        static void Main(string[] args) {
            var cache = new LiveDataCache();
            var handler = new PC2PacketHandler();
            var lapTimeCache = new PC2LapTimeHandler(handler);
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
}
