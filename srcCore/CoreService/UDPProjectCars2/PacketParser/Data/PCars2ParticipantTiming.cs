namespace CoreService.UDPProjectCars2.PacketParser {
    public struct PCars2ParticipantTiming {
        public int participantIndex;
        public short worldPositionX;
        public short worldPositionY;
        public short worldPositionZ;
        public short orientationX;
        public short orientationY;
        public short orientationZ;
        public ushort currentLapDistance;
        public bool active;
        public byte racePosition;
        public int sector;
        public byte highestFlag;
        public byte pitModeSchedule;
        public ushort carIndex;
        public byte raceState;
        public byte currentLap;
        public float currentTime;
        public float currentSectorTime;
    }
}
