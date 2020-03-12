using CoreService.Storage;
using CoreService.Storage.DTOs;

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
    }
}
