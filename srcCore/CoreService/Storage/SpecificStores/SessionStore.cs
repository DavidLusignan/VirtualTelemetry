using CoreService.Data;
using Global.Observable;
using LiteDB;
using System;

namespace CoreService.Storage.SpecificStores {
    public class SessionStore : CollectionStore<SessionEntry> {
        public SessionStore(LiteDatabase db) : base(db) { }

        public void Observe(IObservable<SessionEntry> observable) {
            observable.Subscribe(new Observer<SessionEntry>(OnState));
        }

        private void OnState(SessionEntry session) {
            Upsert(session);
        }
    }
}
