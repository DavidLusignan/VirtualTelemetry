using CoreService.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.F12018 {
    public static class F12018ToStandardDataConverter {
        public static TelemetryState ToTelemetry(F12018Packet packet) {
            var F1Telemetry = (F12018PacketTelemetry)packet.data;
            var throttleRatio = (float)F1Telemetry.throttle.Value / 100f;
            return new TelemetryState(packet.header.sessionTime.Value, throttleRatio, F1Telemetry.speed.Value);
        }
    }
}
