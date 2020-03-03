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
    public class PC2PacketParser : IObservable<DataState>{
        const int DEFAULT_PORT = 5606;
        private List<IObserver<DataState>> observers;
        UdpClient udpClient;
        IPEndPoint ipEndPoint;
        PC2RawHandler pcars2Udp;
        bool keepRunning;
        public PC2PacketParser(int listeningPort = DEFAULT_PORT) {
            observers = new List<IObserver<DataState>>();
            udpClient = new UdpClient(listeningPort);
            ipEndPoint = new IPEndPoint(IPAddress.Any, listeningPort);
            pcars2Udp = new PC2RawHandler(udpClient, ipEndPoint);
        }

        public void Start() {
            keepRunning = true;
            Task.Run(() => {
                while(keepRunning) {
                    HandlePacket();
                }
            });
        }

        private void HandlePacket() {
            try {
                var packet = pcars2Udp.readPackets();
                Task.Run(() => {
                    if (packet != null) {
                        switch (packet.baseUDP.packetType) {
                            case PC2RawHandler.PacketType.Telemetry:
                                var telemetry = (PCars2TelemetryData)packet;
                                NotifyAll(new ViewedParticipantIndexState(telemetry.viewedParticipantIndex));
                                break;
                            case PC2RawHandler.PacketType.Timings:
                                var timings = (PCars2Timings)packet;
                                timings.participants.ForEach(p => { 
                                    var state = new CurrentTimeState(p.currentTime, p.currentLap,
                                        p.participantIndex);
                                    NotifyAll(state);
                                });
                                break;
                            case PC2RawHandler.PacketType.TimeStats:
                                var timeStats = (PCars2TimeStatsData)packet;
                                timeStats.participantStats.ForEach(p => {
                                    var state = new TimeState(p.lastLapTime, p.lastSectorTime, p.participantIndex);
                                    NotifyAll(state);
                                });
                                break;
                        }
                    }
                });
            } catch (Exception e) {
                Console.WriteLine("Error while processing Project Cars 2 udp packets");
                Console.WriteLine(e.Message);
            }
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
