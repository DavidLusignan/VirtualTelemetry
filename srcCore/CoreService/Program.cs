using CoreService.F12018;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using CoreService.UDPProjectCars2.RawPacketHandler;
using Global.Enumerable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace CoreService {
    class Program {
        const int DEFAULT_PORT = 5606;
        static int currentLap = -1;
        static IDictionary<int, float> lapTimes = new Dictionary<int, float>();
        static void Main(string[] args) {
            DTOBsonConversion.Setup();
            var cache = new LiveDataCache();
            var udpClient = new UdpClient(DEFAULT_PORT);
            var ipEndPoint = new IPEndPoint(IPAddress.Any, DEFAULT_PORT);
            var rawHandler = new PC2RawHandler(udpClient, ipEndPoint);
            var packetParser = new PC2PacketParser(rawHandler);
            var db = new LiteDatabase("storage.db");
            var lapTimeCache = new PC2StdLapTimeConvertor(packetParser, db);
            rawHandler.Start();
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
