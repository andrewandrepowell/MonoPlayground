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
        private const float _orientationTimerThreshold = 0.25f;
        private const float _orientationGroundThreshold = 0.25f;
        private const float _jumpTimerThreshold = .35f;
        private const float _jumpEnableTimerThreshold = 0.2f;
        private const float _slideGroundThreshold = 0.25f;
        private readonly SpriteBatch _spriteBatch;
        private readonly PhysicsFeature _physics;
        private readonly AnimationFeature _animationWalk;
        private readonly AnimationFeature _animationIdle;
        private readonly AnimationFeature _animationJump;
        private readonly AnimationFeature _animationFall;
        private readonly AnimationFeature _animationRun;
        private readonly AnimationFeature _animationSlide;
        private readonly float _accelerationMagnitude;
        private readonly Vector2 _gravity;
        private readonly Vector2 _orientationDefault;
        private AnimationFeature _animationCurrent;
        private Vector2 _orientationNormal;
        private Vector2 _jumpOrientation;
        private float _orientationTimer;
        private float _jumpTimer;
        private float _jumpEnableTimer;
        private bool _jumpPressed;
        
        public TestAObject(
            ContentManager contentManager, 
            SpriteBatch spriteBatch,
            Vector2 gravity,
            float friction, 
            float accelerationMagnitude, 
            float maxSpeed,
            float bounce)
        {
            _spriteBatch = spriteBatch;

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
                textures: Enumerable.Range(1, 10).Select(x => contentManager.Load<Texture2D>($"cat/Walk ({x})")).ToList());
            Features.Add(_animationWalk);

            _animationIdle = new AnimationFeature(
                gameObject: this,
                spriteBatch: spriteBatch,
                textures: Enumerable.Range(1, 10).Select(x => contentManager.Load<Texture2D>($"cat/Idle ({x})")).ToList());
            Features.Add(_animationIdle);

            _animationJump = new AnimationFeature(
                gameObject: this,
                spriteBatch: spriteBatch,
                textures: Enumerable.Range(1, 8).Select(x => contentManager.Load<Texture2D>($"cat/Jump ({x})")).ToList());
            Features.Add(_animationJump);

            _animationFall = new AnimationFeature(
                gameObject: this,
                spriteBatch: spriteBatch,
                textures: Enumerable.Range(1, 8).Select(x => contentManager.Load<Texture2D>($"cat/Fall ({x})")).ToList());
            Features.Add(_animationFall);

            _animationSlide = new AnimationFeature(
                gameObject: this,
                spriteBatch: spriteBatch,
                textures: Enumerable.Range(1, 10).Select(x => contentManager.Load<Texture2D>($"cat/Slide ({x})")).ToList());
            Features.Add(_animationSlide);

            _animationRun = new AnimationFeature(
                gameObject: this,
                spriteBatch: spriteBatch,
                textures: Enumerable.Range(1, 8).Select(x => contentManager.Load<Texture2D>($"cat/Run ({x})")).ToList());
            Features.Add(_animationRun);

            float scale = (float)_physics.Mask.Height / _animationWalk.Height;
            Features.OfType<AnimationFeature>().ForEach(delegate(AnimationFeature feature) 
            {
                feature.Scale = scale;
                feature.Rotation = 0;
                feature.AnimationTimerThreshold = 0.1f;
            });
            _animationCurrent = _animationIdle;
            _animationCurrent.Visible = true;
            _animationCurrent.Repeat = true;
            _animationCurrent.Play = true;

            _accelerationMagnitude = accelerationMagnitude;
            _gravity = gravity;
            _orientationDefault = -Vector2.Normalize(gravity);
            _orientationNormal = _orientationDefault;
            _orientationTimer = 0f;
            _jumpEnableTimer = 0f;
            _jumpPressed = false;
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
            else
            {
                Console.WriteLine($"Jump Enable Timer NOT ACTIVATED.");
            }
            Console.WriteLine($"Normal Orientation: {_orientationNormal}. Dot: {dotProduct}");
        }
        private void ChangeAnimation(AnimationFeature animation, bool repeat)
        {
            if (animation == _animationCurrent)
                return;
            _animationCurrent.Play = false;
            _animationCurrent.Visible = false;
            animation.Flip = _animationCurrent.Flip;
            _animationCurrent.Reset();
            _animationCurrent = animation;
            _animationCurrent.Play = true;
            _animationCurrent.Visible = true;
            _animationCurrent.Repeat = repeat;
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 direction = new Vector2(
                x: -_orientationNormal.Y,
                y: _orientationNormal.X);

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
                    if (!_jumpPressed)
                    {
                        _jumpPressed = true;
                        _jumpEnableTimer = 0;
                        _jumpTimer = _jumpTimerThreshold;
                        _jumpOrientation = _orientationNormal;
                    }
                }
                else
                    _jumpPressed = false;
            }

            if (_jumpTimer > 0)
            {
                _jumpTimer -= timeElapsed;
                if (keyboardState.IsKeyDown(Keys.Up))
                    _physics.Acceleration += _jumpOrientation * _accelerationMagnitude * 5;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _physics.Acceleration += direction * _accelerationMagnitude * 2.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _physics.Acceleration -= direction * _accelerationMagnitude * 2.5f;
            }

            // Handle the animations.
            {
                // Animation is flipped based on what key is pressed.
                if (keyboardState.IsKeyDown(Keys.Right))
                    _animationCurrent.Flip = false;
                else if (keyboardState.IsKeyDown(Keys.Left))
                    _animationCurrent.Flip = true;

                // If on the ground--which is what the jump enable timer indicates--then change
                // animation between walking and idle based on whether or not the player
                // is trying to move.
                if (_jumpEnableTimer > 0)
                {
                    float dot = Math.Abs(Vector2.Dot(Vector2.Normalize(_physics.Velocity), direction));
                    if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.Left))
                        ChangeAnimation(animation: _animationRun, repeat: true);
                    else if (dot > _slideGroundThreshold)
                        ChangeAnimation(animation: _animationSlide, repeat: true);
                    else
                        ChangeAnimation(animation: _animationIdle, repeat: true);
                }
                else
                {
                    float dot = Vector2.Dot(_physics.Velocity, _gravity);
                    if (dot < 0)
                        ChangeAnimation(animation: _animationJump, repeat: false);
                    else
                        ChangeAnimation(animation: _animationFall, repeat: true);
                }

                // Set the rotation of the animation based on the orientation normal.
                _animationCurrent.Rotation = (float)Math.Atan2(y: _orientationNormal.Y, x: _orientationNormal.X) + MathHelper.PiOver2;

                // Set the position of the animation based on the position of the physics.
                _animationCurrent.Position = _physics.Position + _physics.Mask.Bounds.Center.ToVector2();
            }

            // Run the other updates.
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            /*
            _spriteBatch.Begin();
            _spriteBatch.DrawLine(
                point1: _physics.CollisionPoint + _orientationNormal * 200,
                point2: _physics.CollisionPoint,
                color: Color.Red,
                thickness: 10);
            _spriteBatch.End();
            */
        }
    }
}
