using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoPlayground
{
    internal static class GameMath
    {
        private static readonly Random _random = new Random();
        public static (T, int) IndexWithMin<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            IEnumerator<T> enumerator = values.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();
            T min = enumerator.Current;
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
            (T min, _) = values.IndexWithMin();
            return min;
        }
        public static T Min<T>(params T[] values) where T : IComparable<T>
        {
            (T min, _) = values.IndexWithMin();
            return min;
        }
        public static int IndexOfMin<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            (_, int index) = values.IndexWithMin();
            return index;
        }
        public static (T, int) IndexWithMax<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            IEnumerator<T> enumerator = values.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException();
            T max = enumerator.Current;
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
            (T max, _) = values.IndexWithMax();
            return max;
        }
        public static T Max<T>(params T[] values) where T : IComparable<T>
        {
            (T max, _) = values.IndexWithMax();
            return max;
        }
        public static int IndexOfMax<T>(this IEnumerable<T> values) where T : IComparable<T>
        {
            (_, int index) = values.IndexWithMax();
            return index;
        }
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (T value in values)
                action(value);
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
