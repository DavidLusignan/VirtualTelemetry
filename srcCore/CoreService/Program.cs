using CoreService.Data;
using CoreService.F12018;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using CoreService.UDPProjectCars2.RawPacketHandler;
using CoreService.UDPProjectCars2.StdDataConvertor;
using Global.Enumerable;
using Global.Observable;
using LiteDB;
using System;

namespace CoreService {
    class Program {
        const int DEFAULT_PORT = 5606;  
        static void Main(string[] args) {
            DTOBsonConversion.Setup();
            var rawHandler = PC2RawHandler.Create(DEFAULT_PORT);
            var packetParser = new PC2PacketParser(rawHandler);
            var db = new LiteDatabase("storage.db");
            var test = new PC2SessionIDPipeline(0, packetParser);
            test.Subscribe(new Observer<SessionState>(session => {
                Console.WriteLine(session);
            }));
            rawHandler.Start();
            while(true){
                
            }
        }
    }
}
