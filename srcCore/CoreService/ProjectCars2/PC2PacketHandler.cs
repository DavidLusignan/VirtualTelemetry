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
                            var lapNumber = pcars2Udp.ParticipantInfo[pcars2Udp.ViewedParticipantIndex, 13];
                            var lastLapTime = pcars2Udp.ParticipantStats[pcars2Udp.ViewedParticipantIndex, 1];
                            var sectorTimes = new Dictionary<int, double>();
                            sectorTimes[0] = pcars2Udp.ParticipantStats[pcars2Udp.ViewedParticipantIndex, 3];
                            sectorTimes[1] = pcars2Udp.ParticipantStats[pcars2Udp.ViewedParticipantIndex, 4];
                            sectorTimes[2] = pcars2Udp.ParticipantStats[pcars2Udp.ViewedParticipantIndex, 5];
                            var lastLapData = new LapData(lastLapTime, sectorTimes);
                            var temp = new TelemetryState(lapNumber, lastLapData);
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
