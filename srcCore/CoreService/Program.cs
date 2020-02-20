using CoreService.Data;
using CoreService.F12018;
using CoreService.ProjectCars2;
using Global.Observable;
using System;
using System.Linq;

namespace CoreService {
    class Program {
        static void Main(string[] args) {
            var cache = new LiveDataCache();
            var handler = new PC2PacketHandler();
            handler.Subscribe(new Observer<DataState>(state => {
                if (state is TelemetryState) {
                    var teleState = (TelemetryState)state;
                    cache.AddData(teleState);
                    cache.AddLapData((int)Math.Floor(teleState.lapNumber) - 1, teleState.previousLapData);
                }
            }));
            handler.Start();
            while(true){
                Console.WriteLine("press enter to dump");
                Console.ReadLine();
                try {
                    var laps = cache.GetLapDatas().OrderBy(kv => kv.Key);
                    foreach(var lap in laps) {
                        Console.WriteLine("Lap Number: {0}; Lap Time: {1}; S1 Time: {2}; S2 Time: {3}; S3 Time: {4}", lap.Key, lap.Value.lapTime, lap.Value.sectorTimes[0], lap.Value.sectorTimes[1], lap.Value.sectorTimes[2]);
                    }
                } catch (Exception e) {
                    Console.WriteLine("Error when outputing");
                }
            }
        }
    }
}
