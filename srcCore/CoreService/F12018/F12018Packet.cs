using CoreService.Data;
using System;

namespace CoreService.F12018 {
    public class F12018Packet {
        public F12018PacketHeader header;
        public F12018PacketData data;

        public F12018Packet(F12018PacketHeader header, F12018PacketData data) {
            this.header = header;
            this.data = data;
        }

        public DataState ToStandardData() {
            var dataType = (F12018PacketType)header.packetId.Value;
            switch(dataType) {
                case F12018PacketType.CAR_TELEMETRY:
                    throw new NotImplementedException();
                case F12018PacketType.MOTION:
                    return F12018ToStandardDataConverter.ToMotion(this);
                default:
                    throw new NotImplementedException();
            }
        }
    }

    enum F12018PacketType {
        MOTION = 0,
        SESSION = 1,
        LAP_DATA = 2,
        EVENT = 3,
        PARTICIPANTS = 4,
        CAR_SETUPS = 5,
        CAR_TELEMETRY = 6,
        CAR_STATUS = 7
    }
}
