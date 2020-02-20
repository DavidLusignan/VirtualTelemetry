using CoreService.Data;
using Global.Observable;
using PcarsUDP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.ProjectCars2 {
    public class PC2PacketHandler : IObservable<DataState>{
        const int DEFAULT_PORT = 5606;
        private List<IObserver<DataState>> observers;
        UdpClient udpClient;
        IPEndPoint ipEndPoint;
        PCars2_UDP pcars2Udp;
        bool keepRunning;
        public PC2PacketHandler(int listeningPort = DEFAULT_PORT) {
            observers = new List<IObserver<DataState>>();
            udpClient = new UdpClient(listeningPort);
            ipEndPoint = new IPEndPoint(IPAddress.Any, listeningPort);
            pcars2Udp = new PCars2_UDP(udpClient, ipEndPoint);
        }

        public void Start() {
            keepRunning = true;
            Task.Run(() => {
                while(keepRunning) {
                    try {
                        pcars2Udp.readPackets();
                        Task.Run(() => {
                            var temp = new TelemetryState(pcars2Udp.ParticipantInfo[0, 14], pcars2Udp.Throttle, pcars2Udp.Speed);
                            observers.ForEach(o => o.OnNext(temp));
                        });
                    } catch (Exception e) {
                        Console.WriteLine("Error while processing Project Cars 2 udp packets");
                    }
                }
            });
        }

        public IDisposable Subscribe(IObserver<DataState> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber<DataState>(observers, observer);
        }
    }
}
