using System;
using System.Collections.Generic;
using System.Linq;

namespace Global.Enumerable {
    public static class EnumerableExtensions {
        public static Dictionary<K, V> ToDictionary<K, V>(this IDictionary<K, V> dic) {
            return dic.ToList().ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            source.Select(i => {
                action(i);
                return 0;
            }).ToList();
        }
    }
}
