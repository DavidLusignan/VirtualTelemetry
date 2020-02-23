using CoreService.Data;
using Global.Enumerable;
using Global.Observable;
using PcarsUDP;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        var packet = pcars2Udp.readPackets();
                        Task.Run(() => {
                            if (packet != null) {
                                switch (packet.baseUDP.packetType) {
                                    case PCars2_UDP.PacketType.Telemetry:
                                        var telemetry = (PCars2TelemetryData)packet;
                                        NotifyAll(new ViewedParticipantIndexState(telemetry.viewedParticipantIndex));
                                        break;
                                    case PCars2_UDP.PacketType.Timings:
                                        var timings = (PCars2Timings)packet;
                                        timings.participants.ForEach(p => NotifyAll(new CurrentTimeState(p.currentTime, p.participantIndex)));
                                        break;
                                    case PCars2_UDP.PacketType.TimeStats:
                                        var timeStats = (PCars2TimeStatsData)packet;
                                        break;
                                }
                            }
                        });
                    } catch (Exception e) {
                        Console.WriteLine("Error while processing Project Cars 2 udp packets");
                    }
                }
            });
        }

        private void NotifyAll(DataState dataState) {
            observers.ForEach(o => o.OnNext(dataState));
        }

        public IDisposable Subscribe(IObserver<DataState> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber<DataState>(observers, observer);
        }
    }
}
