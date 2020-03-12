using CoreService.UDPProjectCars2.StdDataConvertor;
using Global.Observable;
using LiteDB;
using System;

namespace CoreService.Storage.SpecificStores {
    public class SessionTrackInfoStore : CollectionStore<SessionTrackInfo> {
        public SessionTrackInfoStore(LiteDatabase db) : base(db) { }
        public void Observe(IObservable<SessionTrackInfo> observable) {
            observable.Subscribe(new Observer<SessionTrackInfo>(OnState));
        }
        private void OnState(SessionTrackInfo trackInfo) {
            Upsert(trackInfo);
        }
    }
}
