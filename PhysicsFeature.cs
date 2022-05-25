﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class PhysicsFeature : GameFeature
    {
        private readonly Texture2D _mask;
        private readonly Action<PhysicsFeature> _collisionHandle;
        private readonly ICollection<PhysicsFeature> _collidablePhysics;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private float _friction;
        private bool _solid;
        private bool _disposed;
        public PhysicsFeature(GameObject gameObject, ContentManager contentManager, string maskName, Action<PhysicsFeature> collisionHandle) : base(gameObject: gameObject)
        {
            _mask = contentManager.Load<Texture2D>(maskName);
            _collisionHandle = collisionHandle;
            _collidablePhysics = new List<PhysicsFeature>();
            _position = new Vector2(0, 0);
            _velocity = new Vector2(0, 0);
            _acceleration = new Vector2(0, 0);
            _friction = 0f;
            _solid = false;
            _disposed = false;
            // https://codepen.io/OliverBalfour/post/implementing-velocity-acceleration-and-friction-on-a-canvas
        }
        public override void Update(GameTime gameTime)
        {
            _velocity += _acceleration; // Apply acceleration to velocity.
            _velocity = Vector2.Normalize(_velocity) * GameMath.Max(_velocity.Length() - _friction, 0); // Apply friction to velocity.
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds; // Apply velocity to position.
            _collidablePhysics.ForEach(x => Collide(x)); // Apply collision to position and velocity.
        }
        public Texture2D Mask { get => _mask; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }
        public float Friction {  get => _friction; set => _friction = value; }
        public bool Solid {  get => _solid; set => _solid = value; }
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _mask.Dispose();
            }
            _disposed = true;
            base.Dispose(disposing);
        }
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
                    int rowMax = Enumerable
                        .Range(0, thisIntersection.Height)
                        .Select(row => Enumerable
                            .Range(0, thisIntersection.Width)
                            .Select(col => collisionMask[col + row * thisIntersection.Width])
                            .Where(x => x)
                            .Count())
                        .Max();
                    int colMax = Enumerable
                        .Range(0, thisIntersection.Width)
                        .Select(col => Enumerable
                            .Range(0, thisIntersection.Height)
                            .Select(row => collisionMask[col + row * thisIntersection.Width])
                            .Where(x => x)
                            .Count())
                        .Max();

                    if (colMax > rowMax)
                    {
                        if (otherBounds.Top == intersection.Top)
                            _position.Y -= colMax;
                        if (otherBounds.Bottom == intersection.Bottom)
                            _position.Y += colMax;
                    }
                    else
                    {
                        if (otherBounds.Left == intersection.Left)
                            _position.X -= rowMax;
                        if (otherBounds.Right == intersection.Right)
                            _position.X += rowMax;
                    }
                }
                _collisionHandle(other);
            }
        }
    }
}
