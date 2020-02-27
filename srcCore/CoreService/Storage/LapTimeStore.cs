using CoreService.Storage.DTOs;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreService.Storage {
    public interface IStore<T> {
        IEnumerable<T> LoadAll();
        void Store(T item);
        void Update(T item);
        void Delete(Guid id);
    }

    public class CollectionStore<T> : IStore<T> {
        private ILiteCollection<T> _collection;
        public CollectionStore(LiteDatabase db) {
            _collection = db.GetCollection<T>();
        }

        public void Delete(Guid id) {
            var bytes = id.ToByteArray();
            var objectId = new ObjectId(bytes, 4);
            _collection.Delete(objectId);
        }

        public IEnumerable<T> LoadAll() {
            return _collection.FindAll();
        }

        public void Store(T item) {
            _collection.Insert(item);
        }

        public void Update(T item) {
            _collection.Update(item);
        }
    }
}
