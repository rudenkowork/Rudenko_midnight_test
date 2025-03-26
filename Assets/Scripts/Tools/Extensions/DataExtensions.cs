using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools.Extensions
{
    public static class DataExtensions
    {
        public static T ToDeserialized<T>(this string json) => 
            JsonUtility.FromJson<T>(json);

        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);
        
        public static bool IsInRange(this int number, int min, int max) => 
            number >= min && number < max;
        
        public static bool IsEmpty<T>(this IEnumerable<T> e) => !e.Any();

        public static T Random<T>(this IEnumerable<T> e)
        {
            var i = UnityEngine.Random.Range(0, e.Count());
            return e.ElementAt(i);
        }
        
        public static int RandomIndex<T>(this IEnumerable<T> e) 
            => UnityEngine.Random.Range(0, e.Count());

        public static T GetLast<T>(this IEnumerable<T> e)
        {
            var i = e.Count() - 1;
            return e.ElementAt(i);
        }
    }
}