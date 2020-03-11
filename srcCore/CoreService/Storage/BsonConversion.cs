using CoreService.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Storage.DTOs {
    public static class BsonConversion {
        public static void Setup() {
            BsonMapper.Global.RegisterType(
                serialize: entity => ParticipantLapTimes.ToBson(entity),
                deserialize: bson => ParticipantLapTimes.FromBson(bson));
            BsonMapper.Global.RegisterType(
                serialize: entity => SessionState.ToBson(entity),
                deserialize: bson => SessionState.FromBson(bson));
        }
    }
}
