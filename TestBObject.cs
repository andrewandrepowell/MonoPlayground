using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class TestBObject : GameObject
    {
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        public TestBObject(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("object1Mask"),
                collisionHandle: HandleCollision);
            _physics.Physics = false;
            _physics.Solid = true;
            _physics.Vertices.Add(new Vector2(x: 4, y: 3));
            _physics.Vertices.Add(new Vector2(x: 120, y: 3));
            _physics.Vertices.Add(new Vector2(x: 120, y: 121));
            _physics.Vertices.Add(new Vector2(x: 4, y: 121));
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>("object1Mask"),
                spriteBatch: spriteBatch);
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
