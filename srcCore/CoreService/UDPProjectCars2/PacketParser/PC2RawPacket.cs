using System;
using System.Collections.Generic;
using System.Text;
using static PcarsUDP.PC2RawHandler;

namespace CoreService.ProjectCars2 {
    public abstract class PC2RawPacket {
        public PC2RawPacketMeta baseUDP;
    }
}
