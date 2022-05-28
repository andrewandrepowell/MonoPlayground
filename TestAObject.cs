using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
#if DEBUG
using System;
#endif

namespace MonoPlayground
{
    internal class TestAObject : GameObject
    {
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        private readonly float _accelerationMagnitude;
        public TestAObject(ContentManager contentManager, SpriteBatch spriteBatch, float friction, float accelerationMagnitude)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("object0Mask"),
                collisionHandle: HandleCollision);
            _physics.Physics = true;
            _physics.Solid = true;
            _physics.Friction = friction;
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>("object0Mask"),
                spriteBatch: spriteBatch);
            Features.Add(_display);

            _accelerationMagnitude = accelerationMagnitude;
        }
        public PhysicsFeature Physics { get => _physics; }
        private void HandleCollision(PhysicsFeature other)
        {
            Vector2 basis0 = Physics.CollisionNormal;
            Vector2 basis1 = new Vector2(x: -basis0.Y, y: basis0.X);
            float scalar0 = Vector2.Dot(basis0, _physics.Velocity);
            float scalar1 = Vector2.Dot(basis1, _physics.Velocity);
            _physics.Velocity = -0.5f * basis0 * scalar0 + basis1 * scalar1;
            Debug.Assert(!Double.IsNaN(_physics.Velocity.X) && !Double.IsNaN(_physics.Velocity.Y));
            Console.WriteLine($"Scalar0: {scalar0}. Scalar1: {scalar1}.");
        }
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = new Vector2(
                    x: (float)mouseState.X,
                    y: (float)mouseState.Y);
                Vector2 mouseDirection = mousePosition - _physics.Center;
                _physics.Acceleration = Vector2.Normalize(mouseDirection) * _accelerationMagnitude;
            }
            else
            {
                _physics.Acceleration = Vector2.Zero;
            }
            _display.Position = _physics.Position;
            base.Update(gameTime);
        }
    }
}
