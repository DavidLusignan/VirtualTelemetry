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
        public int participantIndex;
        public CurrentTimeState(float currentTime, int participantIndex) {
            this.currentTime = currentTime;
            this.participantIndex = participantIndex;
        }
    }

    public enum DataStateType {
        ViewedParticipant,
        Motion,
        CurrentTime
    }
}
