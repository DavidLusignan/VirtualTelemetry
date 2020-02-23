namespace CoreService.Data {
    public abstract class DataState {
        public abstract DataStateType dataType { get; }
    }

    public class ViewedParticipantIndexState : DataState {
        public override DataStateType dataType => DataStateType.ViewedParticipant;
        public sbyte viewedParticipantIndex;
        public ViewedParticipantIndexState(sbyte viewedParticipantIndex) {
            this.viewedParticipantIndex = viewedParticipantIndex;
        }
    }

    public class CurrentTimeState : DataState {
        public override DataStateType dataType => DataStateType.CurrentTime;
        public float currentTime;
        public byte currentLap;
        public int participantIndex;
        public CurrentTimeState(float currentTime, byte currentLap, int participantIndex) {
            this.currentTime = currentTime;
            this.currentLap = currentLap;
            this.participantIndex = participantIndex;
        }
    }

    public class TimeState : DataState {
        public override DataStateType dataType => DataStateType.Time;
        public float lastLapTime;
        public float lastSectorTime;
        public int participantIndex;
        public TimeState(float lastLapTime, float lastSectorTime, int participantIndex) {
            this.lastLapTime = lastLapTime;
            this.lastSectorTime = lastSectorTime;
            this.participantIndex = participantIndex;
        }
    }

    public enum DataStateType {
        ViewedParticipant,
        Motion,
        CurrentTime,
        Time
    }
}
