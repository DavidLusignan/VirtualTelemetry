using CoreService.F12018;
using CoreService.ProjectCars2;
using CoreService.Storage.DTOs;
using Global.Enumerable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    class Program {
        static int currentLap = -1;
        static IDictionary<int, float> lapTimes = new Dictionary<int, float>();
        static void Main(string[] args) {
            DTOBsonConversion.Setup();
            var cache = new LiveDataCache();
            var handler = new PC2PacketHandler();
            var db = new LiteDatabase("storage.db");
            var lapTimeCache = new PC2LapTimeHandler(handler, db);
            handler.Start();
            while(true){
                Console.WriteLine("Enter to dump");
                Console.ReadLine();
                Console.Clear();
                lapTimeCache.lapTimes.LoadAll().First(lp => lp.participantIndex.Equals(0)).Entity().lapTimes.ForEach(lapTime => {
                    Console.WriteLine("Lap " + lapTime.Key + "; Total : " + lapTime.Value.lapTime + "; S1: " + lapTime.Value.sector1Time + "; S2: " + lapTime.Value.sector2Time + "; S3: " + lapTime.Value.sector3Time);
                });
            }
        }
    }
}
