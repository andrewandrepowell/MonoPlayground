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
        private const float _orientationTimerThreshold = 0.15f;
        private const float _orientationGroundThreshold = 0.25f;
        private const float _jumpTimerThreshold = .25f;
        private const float _jumpEnableTimerThreshold = 0.2f;
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        private readonly float _accelerationMagnitude;
        private readonly Vector2 _gravity;
        private readonly Vector2 _orientationDefault;
        private Vector2 _orientationNormal;
        private Vector2 _jumpOrientation;
        private float _orientationTimer;
        private float _jumpTimer;
        private float _jumpEnableTimer;
        
        public TestAObject(
            ContentManager contentManager, 
            SpriteBatch spriteBatch,
            Vector2 gravity,
            string mask,
            float friction, 
            float accelerationMagnitude, 
            float maxSpeed,
            float bounce)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>(mask),
                collisionHandle: HandleCollision);
            _physics.Physics = true;
            _physics.Solid = true;
            _physics.Friction = friction;
            _physics.MaxSpeed = maxSpeed;
            _physics.Bounce = bounce;
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>(mask),
                spriteBatch: spriteBatch);
            Features.Add(_display);

            _accelerationMagnitude = accelerationMagnitude;
            _gravity = gravity;
            _orientationDefault = -Vector2.Normalize(gravity);
            _orientationNormal = _orientationDefault;
            _orientationTimer = 0f;
            _jumpEnableTimer = 0f;
        }
        public PhysicsFeature Physics { get => _physics; }
        private void HandleCollision(PhysicsFeature other)
        {
            float dotProduct = Vector2.Dot(_orientationNormal, _physics.CollisionNormal);
            if (dotProduct > _orientationGroundThreshold)
            {
                _orientationNormal = _physics.CollisionNormal;
                _orientationTimer = _orientationTimerThreshold;
                _jumpEnableTimer = _jumpEnableTimerThreshold;
                Console.WriteLine($"Jump Enable Timer Activated.");
            }
            Console.WriteLine($"Normal Orientation: {_orientationNormal}. Dot: {dotProduct}");
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _physics.Acceleration = _gravity;

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = new Vector2(
                    x: (float)mouseState.X,
                    y: (float)mouseState.Y);
                Vector2 mouseDirection = mousePosition - _physics.Center;
                _physics.Acceleration += Vector2.Normalize(mouseDirection) * _accelerationMagnitude * 2;
            }

            if (_orientationTimer > 0)
                _orientationTimer -= timeElapsed;
            else
                _orientationNormal = _orientationDefault;

            if (_jumpEnableTimer > 0)
            {
                _jumpEnableTimer -= timeElapsed;

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    _jumpEnableTimer = 0;
                    _jumpTimer = _jumpTimerThreshold;
                    _jumpOrientation = _orientationNormal;
                }
            }

            if (_jumpTimer > 0)
            {
                _jumpTimer -= timeElapsed;
                _physics.Acceleration += _jumpOrientation * _accelerationMagnitude * 5;
            }

            Vector2 direction = new Vector2( 
                x: -_orientationNormal.Y,
                y: _orientationNormal.X);
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _physics.Acceleration += direction * _accelerationMagnitude * 4f;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _physics.Acceleration -= direction * _accelerationMagnitude * 4f;
            }

            _display.Position = _physics.Position;
            base.Update(gameTime);
        }
    }
}
