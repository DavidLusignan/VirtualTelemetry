using Global.Observable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Storage {
    public class ParticipantLapTimesStore : CollectionStore<ParticipantLapTimes> {
        private object _stateLock = new object();
        public ParticipantLapTimesStore(LiteDatabase db) : base(db) { }

        public void Observe(IObservable<ParticipantLapTimes> observable) {
            observable.Subscribe(new Observer<ParticipantLapTimes>(OnState));
        }

        private void OnState(ParticipantLapTimes lapTimes) {
            lock(_stateLock) {
                Store(lapTimes);
            }
        }
    }
}
