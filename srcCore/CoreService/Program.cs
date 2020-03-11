using CoreService.Data;
using CoreService.F12018;
using CoreService.Storage;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using CoreService.UDPProjectCars2.RawPacketHandler;
using CoreService.UDPProjectCars2.StdDataConvertor;
using Global.Enumerable;
using Global.Observable;
using LiteDB;
using System;
using System.Linq;

namespace CoreService {
    class Program {
        const int DEFAULT_PORT = 5606;  
        static void Main(string[] args) {
            DTOBsonConversion.Setup();
            var rawHandler = PC2RawHandler.Create(DEFAULT_PORT);
            var packetParser = new PC2PacketParser(rawHandler);
            var db = new LiteDatabase("storage.db");
            var sessionPipeline = new PC2SessionIDPipeline(0, packetParser);
            var lapTimesStore = new ParticipantLapTimesStore(db);
            var lapTimesPipeline = new PC2StdLapTimePipeline(packetParser, sessionPipeline, lapTimesStore);
            lapTimesStore.Observe(lapTimesPipeline);
            sessionPipeline.Subscribe(new Observer<SessionState>(session => {
                //Console.WriteLine(session);
            }));
            lapTimesPipeline.Subscribe(new Observer<ParticipantLapTimes>(lapTimes => {
                try {
                    //Console.WriteLine(lapTimesStore.LoadAll().First(lt => lt.participantIndex.Equals(0)).lapTimes.Last().Value.lapTime);
                    Console.Clear();
                    Console.WriteLine("ID: " + lapTimes.SessionId);
                    Console.WriteLine("Type: " + lapTimes.SessionType);
                    lapTimes.lapTimes.ForEach(lapTime => {
                        Console.WriteLine("Lap " + lapTime.Key + ": " + lapTime.Value.lapTime);
                    });
                } catch {

                }
            }));
            rawHandler.Start();
            while(true){
                
            }
        }
    }
}
