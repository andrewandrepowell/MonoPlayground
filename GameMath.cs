using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoPlayground
{
    internal static class GameMath
    {
        private static readonly Random _random = new Random();
        private static Texture2D _texture;
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
        // https://community.monogame.net/t/line-drawing/6962
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }

            return _texture;
        }
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
        public static float Cross(Vector2 vector0, Vector2 vector1) => vector0.X * vector1.Y - vector1.X * vector0.X;
    }
}
