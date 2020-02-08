using Global.LazyByteData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.F12018 {
    public class F12018PacketMotion : F12018PacketData {
        public LazyFloat worldPositionX;
        public LazyFloat worldPositionY;
        public LazyFloat worldPositionZ;

        public F12018PacketMotion(byte[] inputBytes) {
            worldPositionX = new LazyFloat(inputBytes, 21);
            worldPositionY = new LazyFloat(inputBytes, 25);
            worldPositionZ = new LazyFloat(inputBytes, 29);
        }
    }
}
