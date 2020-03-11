using CoreService.Data;
using CoreService.Storage;
using CoreService.Storage.DTOs;
using CoreService.Storage.SpecificStores;
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
            BsonConversion.Setup();
            var rawHandler = PC2RawHandler.Create(DEFAULT_PORT);
            var packetParser = new PC2PacketParser(rawHandler);
            var db = new LiteDatabase("storage.db");
            var sessionPipeline = new PC2SessionIDPipeline(0, packetParser);
            var lapTimesStore = new ParticipantLapTimesStore(db);
            var sessionStore = new SessionStore(db);
            var lapTimesPipeline = new PC2StdLapTimePipeline(packetParser, sessionPipeline, lapTimesStore);
            var throttlePipeline = new PC2ThrottlePositionPipeline(packetParser);
            lapTimesStore.Observe(lapTimesPipeline);
            sessionStore.Observe(sessionPipeline);
            rawHandler.Start();
            while(true){
                Console.Clear();
                sessionStore.LoadAll().ForEach(session => {
                    Console.WriteLine("SessionId: {0}; SessionType: {1}", session.Id, session.SessionType);
                });
                Console.ReadLine();
            }
        }
    }
}
