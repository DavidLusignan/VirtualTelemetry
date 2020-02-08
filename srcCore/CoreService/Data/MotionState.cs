using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Data {
    public class MotionState : DataState {
        public float sessionTime;
        public float worldPositionX;
        public float worldPositionY;
        public float worldPositionZ;

        public MotionState(float sessionTime, float worldPositionX, float worldPositionY, float worldPositionZ) {
            this.sessionTime = sessionTime;
            this.worldPositionX = worldPositionX;
            this.worldPositionY = worldPositionY;
            this.worldPositionZ = worldPositionZ;
        }

        public override string ToString() {
            return String.Format("Session Time: {0}; World Position X: {1}, World Position Y: {2}, World Position Z: {3}", sessionTime, worldPositionX, worldPositionY, worldPositionZ);
        }
    }
}
