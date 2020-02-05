using Global.Networking.UDP;
using System;

namespace CoreService {
    class Program {
        static void Main(string[] args) {
            var factory = new F12018PacketFactory();
            var udpReceiver = new VTUDPReceiver(20777, b => {
                var test = factory.CreatePacket(b);
                Console.WriteLine(test.header.packetFormat);
            });
            udpReceiver.Start();
            while(true){}
        }
    }
}
