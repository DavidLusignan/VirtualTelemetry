using CoreService.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.F12018 {
    public static class F12018ToStandardDataConverter {

        public static MotionState ToMotion(F12018Packet packet) {
            var motionData = (F12018PacketMotion)packet.data;
            return new MotionState(packet.header.sessionTime.Value, motionData.worldPositionX.Value, motionData.worldPositionY.Value, motionData.worldPositionZ.Value);
        }
    }
}
