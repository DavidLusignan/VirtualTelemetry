using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Data {
    public class DataState {

    }

    public class TelemetryState : DataState {
        public float sessionTime;
        public float throttleRatio; // 0 to 1
        public float speed;

        public TelemetryState(float sessionTime, float throttleRatio, float speed) {
            this.sessionTime = sessionTime;
            this.throttleRatio = throttleRatio;
            this.speed = speed;
        }

        public override string ToString() {
            return String.Format("Throttle Ratio : {0}; Speed : {1}", throttleRatio, speed);
        }
    }
}
