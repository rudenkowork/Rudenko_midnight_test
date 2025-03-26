using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Extensions
{
    public static class EnumerableExtensions
    {
        private static readonly Random _random = new();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var array = source.ToArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return array;
        }

        public static void Clear<T>(this T[,] arr) where T : class
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = null;
                }
            }
        }
    }
}