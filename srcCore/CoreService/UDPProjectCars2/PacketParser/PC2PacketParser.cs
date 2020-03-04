using CoreService.Data;
using CoreService.UDPProjectCars2.RawPacketHandler;
using Global.Enumerable;
using Global.Observable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.UDPProjectCars2.PacketParser {
    public class PC2PacketParser : IObservable<PC2BasePacket> {
        private List<IObserver<PC2BasePacket>> observers;
        PC2RawHandler pcars2Udp;
        PC2PacketFactory packetFactory = new PC2PacketFactory(8);
        public PC2PacketParser(PC2RawHandler rawHandler) {
            observers = new List<IObserver<PC2BasePacket>>();
            pcars2Udp = rawHandler;
            pcars2Udp.Subscribe(new Observer<PC2RawPacket>(HandlePacket));
        }

        private void HandlePacket(PC2RawPacket rawPacket) {
            NotifyAll(packetFactory.Create(rawPacket));
        }

        private void NotifyAll(PC2BasePacket dataState) {
            observers.ForEach(o => o.OnNext(dataState));
        }

        public IDisposable Subscribe(IObserver<PC2BasePacket> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber<PC2BasePacket>(observers, observer);
        }
    }
}
