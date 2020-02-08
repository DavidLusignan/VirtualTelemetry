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
            return String.Format("Session Time : {0}; Throttle Ratio : {1}; Speed : {2}", sessionTime, throttleRatio, speed);
        }
    }
}
