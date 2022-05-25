using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class PhysicsFeature : GameFeature
    {
        public delegate void Handle(PhysicsFeature other);
        private readonly Texture2D _mask;
        private readonly Handle _handle;
        private readonly ICollection<PhysicsFeature> _collidablePhysics;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        public PhysicsFeature(GameObject gameObject, ContentManager content, string maskName, Handle handle) : base(gameObject: gameObject)
        {
            _mask = content.Load<Texture2D>(maskName);
            _handle = handle;
            _collidablePhysics = new List<PhysicsFeature>();
            _position = new Vector2(0, 0);
            _velocity = new Vector2(0, 0);
            _acceleration = new Vector2(0, 0);
            
        }
        public override void Update(GameTime gameTime)
        {

        }
        public Texture2D Mask { get => _mask; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }
    }
}
