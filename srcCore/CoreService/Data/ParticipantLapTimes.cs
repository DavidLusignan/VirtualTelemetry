using CoreService.Data;
using CoreService.Storage;
using CoreService.Storage.DTOs;
using CoreService.UDPProjectCars2.PacketParser;
using Global.Enumerable;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace CoreService {
    public class ParticipantLapTimes : IStorable {
        public Key Id { get; }
        public int participantIndex;
        public IDictionary<int, ParticipantLapTime> lapTimes;
        public ParticipantLapTimes(Key id, int participantIndex, IDictionary<int, ParticipantLapTime> lapTimes) {
            this.Id = id;
            this.participantIndex = participantIndex;
            this.lapTimes = lapTimes;
        }
        
        public static BsonDocument ParticipantLapTimesToBson(ParticipantLapTimes dto) {
            var bsonDoc = new BsonDocument();
            bsonDoc["_id"] = dto.Id.AsLiteDB();
            bsonDoc["participantIndex"] = new BsonValue(dto.participantIndex);
            bsonDoc["lapTimes"] = new BsonArray(dto.lapTimes.Select(lapTime => {
                var lapTimeDoc = new BsonDocument();
                lapTimeDoc["lapNumber"] = new BsonValue(lapTime.Key);
                lapTimeDoc["lapTime"] = new BsonValue(lapTime.Value.lapTime);
                lapTimeDoc["sector1Time"] = new BsonValue(lapTime.Value.sector1Time);
                lapTimeDoc["sector2Time"] = new BsonValue(lapTime.Value.sector2Time);
                lapTimeDoc["sector3Time"] = new BsonValue(lapTime.Value.sector3Time);
                return lapTimeDoc;
            }).ToArray());
            return bsonDoc;
        }

        public static ParticipantLapTimes BsonToParticipantLapTimes(BsonValue bson) {
            IDictionary<int, ParticipantLapTime> lapTimes;
            if (!bson["lapTimes"].IsNull) {
                lapTimes = bson["lapTimes"].AsArray.Select(lapTimeBson => {
                    var lapNumber = lapTimeBson["lapNumber"].AsInt32;
                    var lapTime = lapTimeBson["lapTime"].AsDouble;
                    var sector1Time = lapTimeBson["sector1Time"].AsDouble;
                    var sector2Time = lapTimeBson["sector2Time"].AsDouble;
                    var sector3Time = lapTimeBson["sector3Time"].AsDouble;
                    return new KeyValuePair<int, ParticipantLapTime>(lapNumber, new ParticipantLapTime(lapTime, sector1Time, sector2Time, sector3Time));
                }).ToDictionary();
            } else {
                lapTimes = new Dictionary<int, ParticipantLapTime>();
            }
            var participantIndex = bson["participantIndex"].AsInt32;
            var id = bson["_id"].AsObjectId;
            return new ParticipantLapTimes(new Key(id), participantIndex, lapTimes);
        }
    }
}
