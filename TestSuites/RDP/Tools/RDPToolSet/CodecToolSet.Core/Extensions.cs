using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodecToolSet.Core
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static T[] Flatten<T>(this T[,] matrix)
        {
            var ret = new List<T>();

            foreach (var row in matrix) {
                ret.Add(row);
            }

            return ret.ToArray();
        }

        public static T[,] ToMatrix<T>(this T[] array, int width)
        {
            var height = (int) Math.Ceiling((double) array.Length/width);
            var matrix = new T[height, width];

            for (var i = 0; i < array.Length; i++) {
                matrix[i/width, i%width] = array[i];
            }

            return matrix;
        }

    }
}
