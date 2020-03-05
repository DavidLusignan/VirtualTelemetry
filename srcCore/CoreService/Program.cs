using CoreService.F12018;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using CoreService.UDPProjectCars2.RawPacketHandler;
using Global.Enumerable;
using Global.Observable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace CoreService {
    class Program {
        const int DEFAULT_PORT = 5606;  
        static IDictionary<int, float> lapTimes = new Dictionary<int, float>();
        static void Main(string[] args) {
            DTOBsonConversion.Setup();
            var cache = new LiveDataCache();
            var rawHandler = PC2RawHandler.Create(DEFAULT_PORT);
            var packetParser = new PC2PacketParser(rawHandler);
            var db = new LiteDatabase("storage.db");
            var lapTimeCache = new PC2StdLapTimeConvertor(packetParser, db);
            lapTimeCache.Subscribe(new Observer<ParticipantLapTimes>(lp => {
                if (lp.participantIndex == 0) {
                    Console.Clear();
                    lp.lapTimes.ForEach(lapTime => {
                        Console.WriteLine("Lap " + lapTime.Key + "; Total : " + lapTime.Value.lapTime + "; S1: " + lapTime.Value.sector1Time + "; S2: " + lapTime.Value.sector2Time + "; S3: " + lapTime.Value.sector3Time);
                    });
                }
            }));
            rawHandler.Start();
            while(true){
                
            }
        }
    }
}
