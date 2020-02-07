using CoreService.F12018;
using Global.Networking.UDP;
using System;

namespace CoreService {
    class Program {
        static void Main(string[] args) {
            var handler = new F12018PacketHandler(telemetry => {
                Console.WriteLine(telemetry);
            });
            handler.Start();
            while(true){

            }
        }
    }
}
