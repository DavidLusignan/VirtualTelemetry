using CoreService.Storage;

namespace CoreService.Data {
    public class SessionState {
        public Key SessionID { get; }
        public SessionType SessionType { get; }
        public SessionProgress SessionProgress { get; }
        public SessionState(Key sessionID, SessionType sessionType, SessionProgress sessionProgress) {
            SessionID = sessionID;
            SessionType = sessionType;
            SessionProgress = sessionProgress;
        }

        public override string ToString() {
            return "SessionID: " + SessionID.ToString() + "; SessionType: " + SessionType + "; SessionProgress: " + SessionProgress;
        }
    }

    public enum SessionType {
        Invalid,
        Practice,
        Qualification,
        Race,
        Test,
        FormationLap,
        TimeAttack
    }

    public enum SessionProgress {
        Invalid,
        NotStarted,
        Racing,
        Finished,
        Disqualified,
        Retired,
        DNF
    }
}
