using CoreService.Storage;
using CoreService.Storage.DTOs;
using LiteDB;
using System;

namespace CoreService.UDPProjectCars2.StdDataConvertor {
    public class SessionTrackInfo : IStorable {
        public Key Id { get; }
        public string TrackName { get; }
        public string TrackVariation { get; }
        public SessionTrackInfo(Key id, string trackName, string trackVariation) {
            Id = id;
            TrackName = trackName;
            TrackVariation = trackVariation;
        }

        internal static BsonValue ToBson(SessionTrackInfo entity) {
            var bsonDoc = new BsonDocument();
            bsonDoc["_id"] = entity.Id.AsLiteDB();
            bsonDoc["trackName"] = entity.TrackName;
            bsonDoc["trackVariation"] = entity.TrackVariation;
            return bsonDoc;
        }

        internal static SessionTrackInfo FromBson(BsonValue bson) {
            var id = bson["_id"].AsObjectId;
            var trackName = bson["trackName"].AsString;
            var trackVariation = bson["trackVariation"].AsString;
            return new SessionTrackInfo(new Key(id), trackName, trackVariation);
        }
    }
}
