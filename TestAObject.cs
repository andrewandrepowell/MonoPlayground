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
        private readonly AnimationFeature _animationWalk;
        private readonly float _accelerationMagnitude;
        private readonly Vector2 _gravity;
        private readonly Vector2 _orientationDefault;
        private AnimationFeature _animationCurrent;
        private Vector2 _orientationNormal;
        private Vector2 _jumpOrientation;
        private float _orientationTimer;
        private float _jumpTimer;
        private float _jumpEnableTimer;
        
        public TestAObject(
            ContentManager contentManager, 
            SpriteBatch spriteBatch,
            Vector2 gravity,
            float friction, 
            float accelerationMagnitude, 
            float maxSpeed,
            float bounce)
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

            _animationWalk = new AnimationFeature(
                gameObject: this,
                spriteBatch: spriteBatch,
                textures: Enumerable.Range(1, 10).Select(x => contentManager.Load<Texture2D>($"cat/Walk ({x})")).ToList(),
                animationTimerThreshold: 0.05f);
            _animationWalk.Visible = true;
            _animationWalk.Repeat = true;
            _animationWalk.Play = true;
            Features.Add(_animationWalk);

            float scale = (float)_physics.Mask.Height / _animationWalk.Height;
            _animationWalk.Scale = scale;
            _animationWalk.Rotation = 0;
            _animationCurrent = _animationWalk;

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
                _animationCurrent.Flip = false;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _physics.Acceleration -= direction * _accelerationMagnitude * 4f;
                _animationCurrent.Flip = true;
            }

            _animationCurrent.Position = _physics.Position + _physics.Mask.Bounds.Center.ToVector2();
            base.Update(gameTime);
        }
    }
}
