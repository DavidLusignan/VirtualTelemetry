using CoreService.UDPProjectCars2.PacketParser;
using Global.Observable;
using System;
using System.Collections.Generic;

namespace CoreService.UDPProjectCars2.StdDataConvertor {
    public class PC2ThrottlePositionPipeline : IObservable<ThrottlePosition> {
        private object _stateLock = new object();
        private List<IObserver<ThrottlePosition>> observers;
        public PC2ThrottlePositionPipeline(IObservable<PC2BasePacket> packetHandler) {
            observers = new List<IObserver<ThrottlePosition>>();
            packetHandler.Subscribe(new Observer<PC2BasePacket>(OnPacket));
        }

        private void OnPacket(PC2BasePacket packet) {
            try {
                if (packet.baseUDP.packetType.Equals(PC2PacketType.Telemetry)) {
                    lock(_stateLock) {
                        var telemetry = (PCars2TelemetryData)packet;
                        NotifyAll(new ThrottlePosition(telemetry.throttle));
                    }
                }
            } catch {

            }
        }
        
        private void NotifyAll(ThrottlePosition throttle) {
            observers.ForEach(o => o.OnNext(throttle));
        }

        public IDisposable Subscribe(IObserver<ThrottlePosition> observer) {
            if (!observers.Contains(observer)) {
                observers.Add(observer);
            }
            return new Unsubscriber<ThrottlePosition>(observers, observer);
        }
    }

    public class ThrottlePosition {
        public double Min = 0d;
        public double Max = 100d;
        public double Value { get; }
        public ThrottlePosition(double value) {
            if (value < Min || value > Max) {
                throw new ArgumentException("Throttle position was not between min and max values");
            }
            Value = value;
        }
    }
}
