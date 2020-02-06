using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.F12018 {
    public class F12018PacketData {

    }

    public class F12018PacketTelemetry : F12018PacketData {
        public ushort speed;
        public byte throttle;

        public F12018PacketTelemetry(ushort speed, byte throttle) {
            this.speed = speed;
            this.throttle = throttle;
        }
    }
}
