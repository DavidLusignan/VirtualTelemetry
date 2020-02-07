using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Data {
    public class TelemetryState {
        public float throttleRatio; // 0 to 1
        public float speed;

        public TelemetryState(float throttleRatio, float speed) {
            this.throttleRatio = throttleRatio;
            this.speed = speed;
        }

        public override string ToString() {
            return String.Format("Throttle Ratio : {0}; Speed : {1}", throttleRatio, speed);
        }
    }
}
