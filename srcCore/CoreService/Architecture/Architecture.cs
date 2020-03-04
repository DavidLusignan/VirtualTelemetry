using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Architecture {
    // This is the packet handler, it receives information from an external API in a certain way (usually UDP or shared mem access)
    interface PacketHandler : IObservable<RawPacket> { }
    struct RawPacket { }

    // This is the packet parser, it listens to notifications from the PacketHandler, reads the data coming in and packages it into a packet 
    // specific to the API
    interface PacketParser : IObservable<DataPacket> { }
    interface PacketFactory {
        DataPacket Create(RawPacket rawPacket);
    }
    struct DataPacket { }

    // This is a data interpretor, it listens to 1..* PacketParser for notifications and creates standard form data notification
    interface DataIntepretor : IObservable<StdData> { }
    interface DataFactory {
        StdData Create(DataPacket dataPacket);
    }
    struct StdData { }
}
