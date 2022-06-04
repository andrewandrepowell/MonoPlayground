using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class TestGeneralWall : GameObject
    {
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        public TestGeneralWall(ContentManager contentManager, Texture2D mask)
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
                texture: mask);
            Features.Add(_display);
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
