using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoPlayground
{
    internal static class GameMath
    {
        private static readonly Random _random = new Random();
        public static (T, int) IndexWithMin<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            T min = values.GetEnumerator().Current;
            int i = 0;
            int index = 0;
            foreach (T value in values)
            {
                if (min.CompareTo(value) > 0)
                {
                    index = i;
                    min = value;
                }
                i++;
            }

            return (min, index);
        }

        public static T Min<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            (T min, _) = IndexWithMin<T>(values);
            return min;
        }

        public static int IndexOfMin<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            (_, int index) = IndexWithMin<T>(values);
            return index;
        }

        public static (T, int) IndexWithMax<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            T max = values.GetEnumerator().Current;
            int i = 0;
            int index = 0;
            foreach (T value in values)
            {
                if (max.CompareTo(value) < 0)
                {
                    index = i;
                    max = value;
                }
                i++;
            }

            return (max, index);
        }

        public static T Max<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            (T max, _) = IndexWithMax<T>(values);
            return max;
        }

        public static int IndexOfMax<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            (_, int index) = IndexWithMax<T>(values);
            return index;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            // https://stackoverflow.com/questions/273313/randomize-a-listt
            int n = list!.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
