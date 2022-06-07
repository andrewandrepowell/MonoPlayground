using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall : GameObject
    {
        public const int Width = 128;
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        public Wall(ContentManager contentManager, Texture2D mask, Texture2D texture = null)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: mask,
                collisionHandle: HandleCollision);
            _physics.Physics = false;
            _physics.Solid = true;
            _physics.Friction = 20;
            _physics.Stick = 0;
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: (texture == null) ? mask : texture);
            Features.Add(_display);

            Debug.Assert((texture != null) ? mask.Bounds == texture.Bounds : true);
            Debug.Assert(mask.Bounds.Width % Width == 0);
            Debug.Assert(mask.Bounds.Height % Width == 0);
        }
        public PhysicsFeature Physics { get => _physics; }
        public override void Update(GameTime gameTime)
        {
            _display.Position = _physics.Position;
            base.Update(gameTime);
        }
        private void HandleCollision(PhysicsFeature other)
        {

        }
    }
}
