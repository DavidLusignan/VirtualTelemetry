using CoreService.UDPProjectCars2.RawPacketHandler;
using Global.Observable;
using System;
using System.Collections.Generic;

namespace CoreService.UDPProjectCars2.PacketParser {
    public class PC2PacketParser : IObservable<PC2BasePacket> {
        private List<IObserver<PC2BasePacket>> observers;
        IObservable<PC2RawPacket> rawPacketObs;
        PC2PacketFactory packetFactory = new PC2PacketFactory(8);
        public PC2PacketParser(IObservable<PC2RawPacket> rawHandler) {
            observers = new List<IObserver<PC2BasePacket>>();
            rawPacketObs = rawHandler;
            rawPacketObs.Subscribe(new Observer<PC2RawPacket>(HandlePacket));
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
