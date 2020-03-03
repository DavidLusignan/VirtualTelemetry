using System;
using System.Collections.Generic;
using System.Linq;

namespace Global.Enumerable {
    public static class EnumerableExtensions {
        public static Dictionary<K, V> ToDictionary<K, V>(this IDictionary<K, V> dic) {
            return dic.ToList().ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> source) {
            return source.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            source.Select(i => {
                action(i);
                return 0;
            }).ToList();
        }

        public static IDictionary<K, V> Except<K, V>(this IDictionary<K, V> source, K key) {
            return source.Where(kv => !kv.Key.Equals(key)).ToDictionary();
        }

        public static Dictionary<K, V> Concat<K, V>(this IDictionary<K, V> source, K key, V value) {
            return source.Concat(new[] { new KeyValuePair<K, V>(key, value) }).ToDictionary();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item) {
            return source.Concat(new[] { item });
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item) {
            return source.Where(s => !s.Equals(item));
        } 
    }
}
