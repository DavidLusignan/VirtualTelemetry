using CoreService.F12018;
using Global.Networking.UDP;
using System;

namespace CoreService {
    class Program {
        static void Main(string[] args) {
            var handler = new F12018PacketHandler(packet => {
                if (packet.header.packetId.Value == 6) {
                    var telemetry = (F12018PacketTelemetry)packet.data;
                    Console.WriteLine(telemetry.throttle);
                }
            });
            handler.Start();
            while(true){

            }
        }
    }
}
