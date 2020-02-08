using CoreService.Data;
using Global.Networking.UDP;
using Global.Observable;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.F12018 {
    public class F12018PacketHandler : IObservable<DataState> {
        const int DEFAULT_PORT = 20777;
        private List<IObserver<DataState>> observers;
        VTUDPReceiver udpReceiver;
        F12018PacketFactory factory;

        public F12018PacketHandler(int listeningPort = DEFAULT_PORT) {
            factory = new F12018PacketFactory();
            observers = new List<IObserver<DataState>>();
            udpReceiver = new VTUDPReceiver(listeningPort, b => {
                var packet = factory.CreatePacket(b);
                var data = packet.ToStandardData();
                observers.ForEach(o => o.OnNext(data));
            });
        }

        public void Start() {
            udpReceiver.Start();
        }

        public void Stop() {
            udpReceiver.Stop();
        }

        public IDisposable Subscribe(IObserver<DataState> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber<DataState>(observers, observer);
        }
    }
}
