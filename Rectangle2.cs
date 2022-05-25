using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoPlayground
{
    internal struct Rectangle2 : IEquatable<Rectangle2>
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float Bottom { get => Y + Width; }
        public Vector2 Center { get => new Vector2(X + Width * 0.5f, Y + Height * 0.5f); }
        public static Rectangle2 Empty { get => new Rectangle2(x: 0f, y: 0f, width: 0f, height: 0f); }
        public bool IsEmpty { get => X == 0f && Y == 0f && Width == 0f && Height == 0f; }
        public float Left { get => X; }
        public float Right { get => X + Width; }
        public float Top { get => Y; }
        public Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }
        public Vector2 Location 
        { 
            get => new Vector2(X, Y); 
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Rectangle2(Vector2 location, Vector2 size)
        {
            X = location.X;
            Y = location.Y; 
            Width = size.X;
            Height = size.Y;
        }
        public Rectangle2(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y; 
            this.Width = width; 
            this.Height = height;
        }
        public Rectangle2(Rectangle rectangle)
        {
            this.X = rectangle.X;
            this.Y = rectangle.Y;
            this.Width = rectangle.Width;
            this.Height = rectangle.Height;
        }
        bool IEquatable<Rectangle2>.Equals(Rectangle2 other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
        }
        public bool Contains(Vector2 value)
        {
            return value.X >= Left && value.X <= Right && value.Y >= Top && value.Y <= Bottom;
        }
        public void Contains(ref Vector2 value, out bool result)
        {
            result = Contains(value);
        }
        public bool Contains(Rectangle2 value)
        {
            return value.Left >= Left && value.Right <= Right && value.Top >= Top && value.Bottom <= Bottom;
        }
        public void Contains(ref Rectangle2 value, out bool result)
        {
            result = Contains(value);
        }
        public bool Contains(float x, float y)
        {
            return x >= Left && x <= Right && y >= Top && y <= Bottom;
        }
        public bool Intersects(Rectangle2 other)
        {
            return Left <= other.Right && Right >= other.Left && Top <= other.Bottom && Bottom <= other.Top;
        }
        public static Rectangle2 Intersect(Rectangle2 value1, Rectangle2 value2)
        {
            Debug.Assert(value1.Intersects(value2));
            float x = GameMath.Max(value1.Left, value2.Left);
            float y = GameMath.Max(value1.Top, value2.Top);
            float width = GameMath.Min(value1.Right, value2.Right) - x;
            float height = GameMath.Min(value1.Bottom, value2.Bottom) - y;
            Rectangle2 intersection = new Rectangle2(x, y, width, height);
            return intersection;
        }
    }
}
