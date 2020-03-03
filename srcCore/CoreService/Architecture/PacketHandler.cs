using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Architecture {
    interface PC2PacketHandler : IObservable<PC2RawPacket> { }
    struct PC2RawPacket { }

    interface PC2PacketParser : IObservable<PC2DataPacket> { }
    interface PC2PacketFactory {
        PC2DataPacket Create(PC2RawPacket rawPacket);
    }
    struct PC2DataPacket { }

    interface PC2DataIntepretor : IObservable<StdData> { }
    interface PC2DataFactory {
        StdData Create(PC2DataPacket dataPacket);
    }
    struct StdData { }
}
