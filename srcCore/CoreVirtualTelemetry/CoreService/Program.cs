using Global.Networking.UDP;
using System;

namespace CoreService {
    class Program {
        static void Main(string[] args) {
            var udpReceiver = new VTUDPReceiver(45001, b => Console.WriteLine("Received some bytes"));
            udpReceiver.Start();
        }
    }
}
