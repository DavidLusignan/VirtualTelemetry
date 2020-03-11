using CoreService.Storage;
using CoreService.Storage.DTOs;

namespace CoreService.Data {
    public class SessionState : IStorable {
        public Key Id { get; }
        public SessionType SessionType { get; }
        public SessionProgress SessionProgress { get; }
        public SessionState(Key id, SessionType sessionType, SessionProgress sessionProgress) {
            Id = id;
            SessionType = sessionType;
            SessionProgress = sessionProgress;
        }

        public override string ToString() {
            return "SessionID: " + Id.ToString() + "; SessionType: " + SessionType + "; SessionProgress: " + SessionProgress;
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
