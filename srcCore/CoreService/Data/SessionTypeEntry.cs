using CoreService.Storage;
using CoreService.Storage.DTOs;
using LiteDB;
using System;

namespace CoreService.Data {
    public class SessionTypeEntry : IStorable {
        public Key Id { get; }
        public SessionType SessionType { get; }
        public SessionTypeEntry(Key id, SessionType sessionType) {
            Id = id;
            SessionType = sessionType;
        }

        public override string ToString() {
            return "SessionID: " + Id.ToString() + "; SessionType: " + SessionType;
        }

        internal static BsonValue ToBson(SessionTypeEntry entity) {
            var bsonDoc = new BsonDocument();
            bsonDoc["_id"] = entity.Id.AsLiteDB();
            bsonDoc["sessionType"] = entity.SessionType.ToString();
            return bsonDoc;
        }

        internal static SessionTypeEntry FromBson(BsonValue bson) {
            var id = bson["_id"].AsObjectId;
            var sessionType = Enum.Parse<SessionType>(bson["sessionType"].AsString);
            return new SessionTypeEntry(new Key(id), sessionType);
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
