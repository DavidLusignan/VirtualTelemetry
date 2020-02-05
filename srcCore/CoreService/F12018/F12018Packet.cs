namespace CoreService {
    class F12018Packet {
        public F12018PacketHeader header;

        public F12018Packet(F12018PacketHeader header) {
            this.header = header;
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
