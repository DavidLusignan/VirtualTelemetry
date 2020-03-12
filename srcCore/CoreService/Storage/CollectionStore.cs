using CoreService.Storage.DTOs;
using Global.Enumerable;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CoreService.Storage {
    public interface IStore<T> {
        T FindWhere(Func<T, bool> predicate);
        bool ExistsWhere(Func<T, bool> predicate);
        IEnumerable<T> LoadAll();
        void Store(T item);
        void Upsert(T item);
        void Delete(Key key);
        void DeleteAll();
    }

    public class CollectionStore<T> : IStore<T> where T : IStorable {
        protected ILiteCollection<T> _collection;
        public CollectionStore(LiteDatabase db) {
            _collection = db.GetCollection<T>();
        }

        public void Delete(Key key) {
            var bytes = key.AsLiteDB().ToByteArray();
            var objectId = new ObjectId(bytes, 4);
            _collection.Delete(objectId);
        }

        public void DeleteAll() {
            var all = _collection.FindAll();
            all.ForEach(doc => _collection.Delete(doc.Id.AsLiteDB()));
        }

        public IEnumerable<T> LoadAll() {
            return _collection.FindAll();
        }

        public bool ExistsWhere(Func<T, bool> predicate) {
            try {
                return _collection.FindAll().Any(predicate);
            } catch (Exception e) {
                return false;
            }
        }

        public T FindWhere(Func<T, bool> predicate) {
            return _collection.FindAll().First(predicate);
        }

        public void Store(T item) {
            _collection.Insert(item.Id.AsLiteDB(), item);
        }

        public void Upsert(T item) {
            _collection.Upsert(item.Id.AsLiteDB(), item);
        }
    }
}
