using CoreService.Storage;
using CoreService.Storage.DTOs;
using LiteDB;
using System;

namespace CoreService.Data {
    public class SessionEntry : IStorable {
        public Key Id { get; }
        public SessionType SessionType { get; } 
        public DateTime Beginning { get; }
        public DateTime? End { get; }
        public SessionEntry(Key id, SessionType sessionType, DateTime start, DateTime? end) {
            Id = id;
            SessionType = sessionType;
            Beginning = start;
            End = end;
        }

        public override string ToString() {
            return "SessionID: " + Id.ToString() + "; SessionType: " + SessionType + "; Start: " + Beginning;
        }

        internal static BsonValue ToBson(SessionEntry entity) {
            var bsonDoc = new BsonDocument();
            bsonDoc["_id"] = entity.Id.AsLiteDB();
            bsonDoc["sessionType"] = entity.SessionType.ToString();
            bsonDoc["beginning"] = entity.Beginning;
            if (entity.End.HasValue) {
                bsonDoc["end"] = entity.End.Value;
            }
            return bsonDoc;
        }

        internal static SessionEntry FromBson(BsonValue bson) {
            var id = bson["_id"].AsObjectId;
            var sessionType = Enum.Parse<SessionType>(bson["sessionType"].AsString);
            var start = bson["beginning"].AsDateTime;
            DateTime? end;
            if (!bson["end"].IsNull) {
                end = bson["end"].AsDateTime;
            } else {
                end = null;
            }
            return new SessionEntry(new Key(id), sessionType, start, end);
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
