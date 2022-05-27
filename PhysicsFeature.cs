using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace MonoPlayground
{
    internal class PhysicsFeature : GameFeature
    {
        private readonly Texture2D _mask;
        private readonly Action<PhysicsFeature> _collisionHandle;
        private readonly ICollection<PhysicsFeature> _collidablePhysics;
        private Vector2 _collisionPoint;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private float _friction;
        private bool _solid;
        private bool _physics;
        public PhysicsFeature(GameObject gameObject, Texture2D mask, Action<PhysicsFeature> collisionHandle) : base(gameObject: gameObject)
        {
            _mask = mask;
            _collisionHandle = collisionHandle;
            _collidablePhysics = new List<PhysicsFeature>();
            _collisionPoint = Vector2.Zero;
            _position = Vector2.Zero;
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
            _friction = 0f;
            _solid = false;
            _physics = false;
            // https://codepen.io/OliverBalfour/post/implementing-velocity-acceleration-and-friction-on-a-canvas
        }
        public override void Update(GameTime gameTime)
        {
            if (!_physics)
                return;
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _velocity += _acceleration * timeElapsed; // Apply acceleration to velocity.
            if (_velocity != Vector2.Zero)
                _velocity = Vector2.Normalize(_velocity) * GameMath.Max(_velocity.Length() - _friction * timeElapsed, 0); // Apply friction to velocity.
            _position += _velocity * timeElapsed; // Apply velocity to position.
            _collidablePhysics.ForEach(x => Collide(x)); // Apply collision to position and velocity.
#if DEBUG
            AllocConsole();
            Console.WriteLine($"Position={_position}");
#endif 
        }
        public override void Draw(GameTime gameTime) { }
        public Texture2D Mask { get => _mask; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 Center { get => _position + _mask.Bounds.Center.ToVector2(); }
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }
        public float Friction {  get => _friction; set => _friction = value; }
        public bool Solid {  get => _solid; set => _solid = value; }
        public bool Physics { get => _physics; set => _physics = value; }
        public Vector2 CollisionPoint { get => _collisionPoint; }
        public ICollection<PhysicsFeature> CollidablePhysics { get => _collidablePhysics; }
        private void Collide(PhysicsFeature other)
        {
            Rectangle thisBounds = new Rectangle(
                location: _position.ToPoint(),
                size: _mask.Bounds.Size);
            Rectangle otherBounds = new Rectangle(
                location: other.Position.ToPoint(),
                size: other.Mask.Bounds.Size);

            if (!thisBounds.Intersects(otherBounds))
                return;

            Rectangle intersection = Rectangle.Intersect(thisBounds, otherBounds);
            Rectangle thisIntersection = new Rectangle(
                x: intersection.X - thisBounds.X,
                y: intersection.Y - thisBounds.Y,
                width: intersection.Width,
                height: intersection.Height);
            Rectangle otherIntersection = new Rectangle(
                x: intersection.X - otherBounds.X,
                y: intersection.Y - otherBounds.Y,
                width: intersection.Width,
                height:intersection.Height);
            int Area = intersection.Height * intersection.Width;

            Color[] thisData = new Color[Area];
            Color[] otherData = new Color[Area];

            _mask.GetData(
                level: 0,
                rect: thisIntersection,
                data: thisData,
                startIndex: 0,
                elementCount: thisData.Length);
            other.Mask.GetData(
                level: 0,
                rect: otherIntersection,
                data: otherData,
                startIndex: 0,
                elementCount: otherData.Length);

            bool[] collisionMask = thisData.Zip(otherData, (td, od) => td != Color.Transparent && od != Color.Transparent).ToArray();
            bool collisionOccurred = collisionMask.Contains(true);

            if (collisionOccurred)
            {
                if (_solid && other.Solid)
                {
                    (int rowMax, int rowIndex) = Enumerable
                        .Range(0, thisIntersection.Height)
                        .Select(row => Enumerable
                            .Range(0, thisIntersection.Width)
                            .Select(col => collisionMask[col + row * thisIntersection.Width])
                            .Where(x => x)
                            .Count())
                        .IndexWithMax();
                    (int colMax, int colIndex) = Enumerable
                        .Range(0, thisIntersection.Width)
                        .Select(col => Enumerable
                            .Range(0, thisIntersection.Height)
                            .Select(row => collisionMask[col + row * thisIntersection.Width])
                            .Where(x => x)
                            .Count())
                        .IndexWithMax();

                    if (colMax < rowMax)
                    {
                        _collisionPoint.X = intersection.X + colIndex;
                        if (otherBounds.Top == intersection.Top)
                        {
                            _position.Y -= colMax;
                            _collisionPoint.Y = intersection.Top - 1;
                        }
                        else if (otherBounds.Bottom == intersection.Bottom)
                        {
                            _position.Y += colMax;
                            _collisionPoint.Y = intersection.Bottom + 1;
                        }
                    }
                    else
                    {
                        _collisionPoint.Y = intersection.Y + rowIndex;
                        if (otherBounds.Left == intersection.Left)
                        {
                            _position.X -= rowMax;
                            _collisionPoint.X = intersection.Left - 1;
                        }
                        else if (otherBounds.Right == intersection.Right)
                        {
                            _position.X += rowMax;
                            _collisionPoint.X = intersection.Right + 1;
                        }
                    }
                }
                _collisionHandle(other);
            }
        }
#if DEBUG
        // https://gamedev.stackexchange.com/questions/45107/input-output-console-window-in-xna#:~:text=Right%20click%20your%20game%20in%20the%20solution%20explorer,tab.%20Change%20the%20Output%20Type%20to%20Console%20Application.
        [DllImport("kernel32")]
        static extern bool AllocConsole();
#endif
    }
}
