using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class PhysicsFeature : GameFeature
    {
        private readonly Texture2D _maskTexture;
        private readonly Rectangle2 _maskBounds;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        public PhysicsFeature(GameObject gameObject, ContentManager content, string maskName) : base(gameObject: gameObject)
        {
            _maskTexture = content.Load<Texture2D>(maskName);
            _maskBounds = new Rectangle2(_maskTexture.Bounds);
            _position = new Vector2(0, 0);
            _velocity = new Vector2(0, 0);
            _acceleration = new Vector2(0, 0);
        }
        public Texture2D MaskTexture { get => _maskTexture; }
        public Rectangle2 MaskBounds { get => _maskBounds; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }
    }
}
