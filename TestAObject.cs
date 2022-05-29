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
        private readonly Vector2 _gravity;
        public TestAObject(
            ContentManager contentManager, 
            SpriteBatch spriteBatch, 
            float friction, 
            float accelerationMagnitude, 
            float maxSpeed,
            float bounce,
            Vector2 gravity)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("object0Mask"),
                collisionHandle: HandleCollision);
            _physics.Physics = true;
            _physics.Solid = true;
            _physics.Friction = friction;
            _physics.MaxSpeed = maxSpeed;
            _physics.Bounce = bounce;
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>("object0Mask"),
                spriteBatch: spriteBatch);
            Features.Add(_display);

            _accelerationMagnitude = accelerationMagnitude;
            _gravity = gravity;
        }
        public PhysicsFeature Physics { get => _physics; }
        private void HandleCollision(PhysicsFeature other)
        {

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
            _physics.Acceleration += _gravity;

            _display.Position = _physics.Position;
            base.Update(gameTime);
        }
    }
}
