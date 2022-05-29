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
        private readonly float _orientationThreshold;
        private Vector2 _normalOrientation;
        private float _orientationTimer;
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
            _physics.Bounce = 0f;
            _physics.Stick = 4;
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>("object0Mask"),
                spriteBatch: spriteBatch);
            Features.Add(_display);

            _accelerationMagnitude = accelerationMagnitude;
            _gravity = gravity;
            _normalOrientation = new Vector2(x: 0, y: -1);
            _orientationThreshold = .25f;
            _orientationTimer = 0f;
        }
        public PhysicsFeature Physics { get => _physics; }
        private void HandleCollision(PhysicsFeature other)
        {
            float dotProduct = Vector2.Dot(_normalOrientation, _physics.CollisionNormal);
            float crossProduct = GameMath.Cross(_normalOrientation, _physics.CollisionNormal);
            if (dotProduct > .5)
            {
                _normalOrientation = _physics.CollisionNormal;
                _orientationTimer = 0;
            }
            Console.WriteLine($"Normal Orientation: {_normalOrientation}. Dot: {dotProduct}: Cross: {crossProduct}");
        }
        public override void Update(GameTime gameTime)
        {
            _physics.Acceleration = _gravity;

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = new Vector2(
                    x: (float)mouseState.X,
                    y: (float)mouseState.Y);
                Vector2 mouseDirection = mousePosition - _physics.Center;
                _physics.Acceleration += Vector2.Normalize(mouseDirection) * _accelerationMagnitude;
            }

            _orientationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_orientationTimer > _orientationThreshold)
            {
                _orientationTimer = 0;
                _normalOrientation = new Vector2(x: 0, y: -1);
            }

            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 direction = new Vector2( 
                x: -_normalOrientation.Y,
                y: _normalOrientation.X);
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _physics.Acceleration += direction * _accelerationMagnitude / 3 * 2;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _physics.Acceleration -= direction * _accelerationMagnitude / 3 * 2;
            }

            _display.Position = _physics.Position;
            base.Update(gameTime);
        }
    }
}
