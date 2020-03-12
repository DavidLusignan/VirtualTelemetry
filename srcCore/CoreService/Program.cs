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
            var sessionPipeline = new PC2SessionTypePipeline(packetParser);
            var lapTimesStore = new ParticipantLapTimesStore(db);
            var sessionStore = new SessionStore(db);
            var trackInfoStore = new SessionTrackInfoStore(db);
            var lapTimesPipeline = new PC2StdLapTimePipeline(packetParser, sessionPipeline, lapTimesStore);
            var throttlePipeline = new PC2ThrottlePositionPipeline(packetParser);
            var trackInfoPipeline = new PC2SessionTrackInfoPipeline(packetParser, sessionPipeline);
            lapTimesStore.Observe(lapTimesPipeline);
            sessionStore.Observe(sessionPipeline);
            trackInfoStore.Observe(trackInfoPipeline);
            rawHandler.Start();
            while(true){
                try {
                    Console.Clear();
                    sessionStore.LoadAll().ForEach(session => {
                        try {
                            var track = trackInfoStore.LoadWithId(session.Id);
                            Console.WriteLine("SessionId: {0}; SessionType: {1}; Track: {2} {3}", session.Id, session.SessionType, track.TrackName, track.TrackVariation);
                        } catch {
                            
                        }
                    });
                    Console.ReadLine();
                } catch {

                }
            }
        }
    }
}
