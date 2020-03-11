using CoreService.Data;
using Global.Observable;
using LiteDB;
using System;

namespace CoreService.Storage.SpecificStores {
    public class SessionStore : CollectionStore<SessionState> {
        public SessionStore(LiteDatabase db) : base(db) { }

        public void Observe(IObservable<SessionState> observable) {
            observable.Subscribe(new Observer<SessionState>(OnState));
        }

        private void OnState(SessionState session) {
            Upsert(session);
        }
    }
}
