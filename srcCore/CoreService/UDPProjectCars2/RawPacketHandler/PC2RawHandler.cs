using Global.Observable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CoreService.UDPProjectCars2.RawPacketHandler {
    public class PC2RawHandler : IObservable<PC2RawPacket>{
        private List<IObserver<PC2RawPacket>> _observers;
        private bool _keepRunning;
        private UdpClient _listener;
        private IPEndPoint _groupEP;

        public PC2RawHandler(UdpClient listener, IPEndPoint groupEp) {
            _observers = new List<IObserver<PC2RawPacket>>();
            _listener = listener;
            _groupEP = groupEp;
        }

        public void Start() {
            _keepRunning = true;
            Task.Run(() => {
                while(_keepRunning) {
                    try {
                        HandlePacket();
                    } catch {
                        
                    }
                }
            });
        }

        private void HandlePacket() {
            byte[] udpPacket = _listener.Receive(ref _groupEP);
            Stream stream = new MemoryStream(udpPacket);
            var packet = PC2RawPacket.Create(stream);
            _observers.ForEach(obs => obs.OnNext(packet));
        }

        public IDisposable Subscribe(IObserver<PC2RawPacket> observer) {
            if (!_observers.Contains(observer)) {
                _observers.Add(observer);
            }
            return new Unsubscriber<PC2RawPacket>(_observers, observer);
        }

        public static PC2RawHandler Create(int port) {
            var udpClient = new UdpClient(port);
            var ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            return new PC2RawHandler(udpClient, ipEndPoint);
        }
    }
}
