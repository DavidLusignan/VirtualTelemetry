using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Architecture {
    // This is the packet handler, it receives information from an external API in a certain way (usually UDP or shared mem access)
    // It only reads the bytes of information 
    interface PacketHandler : IObservable<RawPacket> { }
    struct RawPacket { }

    interface PacketParser : IObservable<DataPacket> { }
    interface PacketFactory {
        DataPacket Create(RawPacket rawPacket);
    }
    struct DataPacket { }

    interface DataIntepretor : IObservable<StdData> { }
    interface DataFactory {
        StdData Create(DataPacket dataPacket);
    }
    struct StdData { }
}
