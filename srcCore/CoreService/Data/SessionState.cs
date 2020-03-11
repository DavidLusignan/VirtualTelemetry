using CoreService.Storage;
using CoreService.Storage.DTOs;
using LiteDB;
using System;

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

        internal static BsonValue ToBson(SessionState entity) {
            var bsonDoc = new BsonDocument();
            bsonDoc["_id"] = entity.Id.AsLiteDB();
            bsonDoc["sessionType"] = entity.SessionType.ToString();
            bsonDoc["sessionProgress"] = entity.SessionProgress.ToString();
            return bsonDoc;
        }

        internal static SessionState FromBson(BsonValue bson) {
            var id = bson["_id"].AsObjectId;
            var sessionType = Enum.Parse<SessionType>(bson["sessionType"].AsString);
            var sessionProgress = Enum.Parse<SessionProgress>(bson["sessionProgress"].AsString);
            return new SessionState(new Key(id), sessionType, sessionProgress);
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
