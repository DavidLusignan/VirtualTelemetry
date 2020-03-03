using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Storage.DTOs {
    public static class DTOBsonConversion {
        public static void Setup() {
            BsonMapper.Global.RegisterType<ParticipantLapTimesDTO>(
                serialize: (dto) => ParticipantLapTimesDTO.ParticipantLapTimesDTOToBson(dto),
                deserialize: (bson) => ParticipantLapTimesDTO.BsonToParticipantLapTimesDTO(bson));
        }
    }
}
