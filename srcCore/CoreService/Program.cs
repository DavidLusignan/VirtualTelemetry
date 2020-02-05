using CoreService.F12018;
using Global.Networking.UDP;
using System;

namespace CoreService {
    class Program {
        static void Main(string[] args) {
            var factory = new F12018PacketFactory();
            var udpReceiver = new VTUDPReceiver(20777, b => {
                var test = factory.CreatePacket(b);
                if (test.header.packetId == 6) {
                    var data = (F12018PacketTelemetry)test.data;
                    Console.WriteLine(data.speed);
                }
            });
            udpReceiver.Start();
            while(true){}
        }
    }
}
