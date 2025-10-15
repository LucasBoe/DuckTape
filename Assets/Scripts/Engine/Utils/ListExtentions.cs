using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public static class ListExtentions
    {
        public static bool AddUnique<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
                return false;

            list.Add(item);
            return true;
        }
        public static bool AddUnique(this List<object> list, object item)
        {
            if (list.Contains(item))
                return false;

            list.Add(item);
            return true;
        }
        public static T PickRandom<T>(this IList<T> list)
        {
            if (list.Count == 0)
                return default;
            
            return list[Random.Range(0, list.Count)];
        }
        public static void AddMultipleNew<T>(this List<T> list, int amount) where T : new()
        {
            for (int i = 0; i < amount; i++)
            {
                list.Add(new T());
            }
        }
        public static List<T> GetOrCreateEntry<TKey, T>(this Dictionary<TKey, List<T>> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new List<T>());

            return dictionary[key];
        }

        public static void AddOrModifyAtKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }
    }
}