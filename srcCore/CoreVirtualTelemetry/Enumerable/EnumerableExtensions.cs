using System.Collections.Generic;
using System.Linq;

namespace Global.Enumerable {
    public static class EnumerableExtensions {
        public static Dictionary<K, V> ToDictionary<K, V>(this Dictionary<K, V> dic) {
            return dic.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
