using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Storage.DTOs {
    public static class BsonConversion {
        public static void Setup() {
            BsonMapper.Global.RegisterType<ParticipantLapTimes>(
                serialize: (dto) => ParticipantLapTimes.ParticipantLapTimesToBson(dto),
                deserialize: (bson) => ParticipantLapTimes.BsonToParticipantLapTimes(bson));
        }
    }
}
