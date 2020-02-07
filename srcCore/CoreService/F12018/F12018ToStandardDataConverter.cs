using CoreService.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.F12018 {
    public static class F12018ToStandardDataConverter {
        public static TelemetryState ToTelemetry(F12018PacketData data) {
            var F1Telemetry = (F12018PacketTelemetry)data;
            var throttleRatio = (float)F1Telemetry.throttle.Value / 100f;
            return new TelemetryState(throttleRatio, F1Telemetry.speed.Value);
        }
    }
}
