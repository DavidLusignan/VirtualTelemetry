using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Data {
    public class TelemetryState : DataState {
        public double sessionTime;
        public double throttleRatio; // 0 to 1
        public double speed;

        public TelemetryState(double sessionTime, double throttleRatio, double speed) {
            this.sessionTime = sessionTime;
            this.throttleRatio = throttleRatio;
            this.speed = speed;
        }

        public override string ToString() {
            return String.Format("Session Time: {0}; Throttle Ratio: {1}; Speed: {2}", sessionTime, throttleRatio, speed);
        }
    }
}
