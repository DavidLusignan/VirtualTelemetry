using LiteDB;
using System;
using System.Collections.Generic;

namespace CoreService.Storage {
    public class Key {
        private ObjectId id;

        internal Key(ObjectId id) {
            this.id = id;
        }

        internal ObjectId AsLiteDB() {
            return id;
        }

        public static Key Create() {
            return new Key(ObjectId.NewObjectId());
        }

        public override bool Equals(object obj) {
            return obj is Key key &&
                   EqualityComparer<ObjectId>.Default.Equals(id, key.id);
        }

        public override int GetHashCode() {
            return HashCode.Combine(id);
        }
    }
}
