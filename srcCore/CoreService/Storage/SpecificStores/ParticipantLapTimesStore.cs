using Global.Observable;
using LiteDB;
using System;

namespace CoreService.Storage.SpecificStores {
    public class ParticipantLapTimesStore : CollectionStore<ParticipantLapTimes> {
        public ParticipantLapTimesStore(LiteDatabase db) : base(db) { }

        public void Observe(IObservable<ParticipantLapTimes> observable) {
            observable.Subscribe(new Observer<ParticipantLapTimes>(OnState));
        }

        private void OnState(ParticipantLapTimes lapTimes) {
            Upsert(lapTimes);
        }
    }
}
