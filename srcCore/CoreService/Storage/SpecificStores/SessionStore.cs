using CoreService.Data;
using Global.Observable;
using LiteDB;
using System;

namespace CoreService.Storage.SpecificStores {
    public class SessionStore : CollectionStore<SessionTypeEntry> {
        public SessionStore(LiteDatabase db) : base(db) { }

        public void Observe(IObservable<SessionTypeEntry> observable) {
            observable.Subscribe(new Observer<SessionTypeEntry>(OnState));
        }

        private void OnState(SessionTypeEntry session) {
            Upsert(session);
        }
    }
}
