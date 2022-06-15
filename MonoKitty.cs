using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace MonoPlayground
{
    internal class MonoKitty : GameObject
    {
        private const float _stickGround = 120;
        private const float _stickDefault = 0;
        private const float _orientationGroundThreshold = 0.25f;
        private const float _jumpTimerThreshold = .25f;
        private const float _jumpEnableTimerThreshold = 0.1f;
        private const float _jumpAccelerationScale = 8;
        private const float _slideGroundThreshold = 0.25f;
        private const float _runAccelerationScale = 2.5f;
        private const float _accelerationMagnitude = 1000f;
        private const float _bouncerThreshold = .70f;
        private const float _bouncerTimerThreshold = .60f;
        private const float _bouncerBounce = 100f;
        private const float _bouncerAccelerationScale = 8f;
        private static readonly Vector2 _gravity = new Vector2(x: 0, y: 2000);
        private static readonly Vector2 _orientationDefault = -Vector2.Normalize(_gravity);
        private readonly PhysicsFeature _physics;
        private readonly AnimationFeature _animationWalk;
        private readonly AnimationFeature _animationIdle;
        private readonly AnimationFeature _animationJump;
        private readonly AnimationFeature _animationFall;
        private readonly AnimationFeature _animationRun;
        private readonly AnimationFeature _animationSlide;
        private readonly SoundEffectInstance _soundRun;
        private readonly SoundEffectInstance _soundLand;
        private readonly SoundEffectInstance _soundSlide;
        private AnimationFeature _animationCurrent;
        private Vector2 _orientationNormal;
        private Vector2 _jumpOrientation;
        private Vector2 _bouncerOrientation;
        private float _jumpTimer;
        private float _jumpEnableTimer;
        private bool _jumpPressed;
        private float _bouncerTimer;
        
        public MonoKitty(ContentManager contentManager)
        {
            // Construct the physics.
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("object0Mask"),
                collisionHandle: HandleCollision);
            _physics.Physics = true;
            _physics.Solid = true;
            _physics.Friction = 600;
            _physics.MaxSpeed = 600;
            _physics.Bounce = 0;
            _physics.StickThreshold = 120f;
            _physics.Stick = _stickDefault;
            Features.Add(_physics);

            // Construct all the animations.
            {
                _animationWalk = new AnimationFeature(
                    gameObject: this,
                    textures: Enumerable.Range(1, 10).Select(x => contentManager.Load<Texture2D>($"cat/Walk ({x})")).ToList());
                Features.Add(_animationWalk);

                _animationIdle = new AnimationFeature(
                    gameObject: this,
                    textures: Enumerable.Range(1, 10).Select(x => contentManager.Load<Texture2D>($"cat/Idle ({x})")).ToList());
                Features.Add(_animationIdle);

                _animationJump = new AnimationFeature(
                    gameObject: this,
                    textures: Enumerable.Range(1, 8).Select(x => contentManager.Load<Texture2D>($"cat/Jump ({x})")).ToList());
                Features.Add(_animationJump);

                _animationFall = new AnimationFeature(
                    gameObject: this,
                    textures: Enumerable.Range(1, 8).Select(x => contentManager.Load<Texture2D>($"cat/Fall ({x})")).ToList());
                Features.Add(_animationFall);

                _animationSlide = new AnimationFeature(
                    gameObject: this,
                    textures: Enumerable.Range(1, 10).Select(x => contentManager.Load<Texture2D>($"cat/Slide ({x})")).ToList());
                Features.Add(_animationSlide);

                _animationRun = new AnimationFeature(
                    gameObject: this,
                    textures: Enumerable.Range(1, 8).Select(x => contentManager.Load<Texture2D>($"cat/Run ({x})")).ToList());
                Features.Add(_animationRun);

                float scale = 1.1f * (float)_physics.Mask.Height / _animationWalk.Height;
                Features.OfType<AnimationFeature>().ForEach(delegate (AnimationFeature feature)
                {
                    feature.Scale = scale;
                    feature.Rotation = 0;
                    feature.AnimationTimerThreshold = 0.1f;
                });
                _animationRun.AnimationTimerThreshold = 0.05f;
                _animationCurrent = _animationIdle;
                _animationCurrent.Visible = true;
                _animationCurrent.Repeat = true;
                _animationCurrent.Play = true;
            }

            // Construct all the sounds.
            {
                _soundRun = contentManager.Load<SoundEffect>("boopSound").CreateInstance();
                _soundRun.Volume = 0.01f;
                _soundRun.Pitch = -0.8f;
                _soundLand = contentManager.Load<SoundEffect>("boopSound").CreateInstance();
                _soundLand.Volume = 0.01f;
                _soundSlide = contentManager.Load<SoundEffect>("noiseSound").CreateInstance();
                _soundSlide.Volume = 0.05f;
                _soundSlide.Pitch = -0.9f;
            }

            // Initialize leftover properties.
            _orientationNormal = _orientationDefault;
            _jumpEnableTimer = 0f;
            _jumpPressed = false;
            _bouncerOrientation = _orientationDefault;
            _bouncerTimer = 0f;
            ExternalAcceleration = Vector2.Zero;
        }
        public PhysicsFeature Physics { get => _physics; }
        public Vector2 ExternalAcceleration { get; set; }
        private void HandleCollision(PhysicsFeature other)
        {
            // Detect if collision with ground. Ground is any object below the player's orientation.
            {
                float dot = Vector2.Dot(_orientationNormal, _physics.CollisionNormal);
                if (dot > _orientationGroundThreshold)
                {
                    // If previously in air, play landing sound.
                    if (_jumpEnableTimer <= 0)
                        _soundLand.Play();

                    // Orientation normal of the player is the collision normal.
                    // The jump enable timer indicates the player is still on the ground.
                    _orientationNormal = _physics.CollisionNormal;
                    _jumpEnableTimer = _jumpEnableTimerThreshold;

                    
                }
            }

            // There's a special case if other is a bouncer.
            {
                Bouncer bouncer = other.GameObject as Bouncer;
                if (bouncer != null)
                {
                    // Check and see if player is on top of bouncer.
                    float dot = Vector2.Dot(_physics.CollisionNormal, bouncer.Direction);
                    if (dot >= _bouncerThreshold)
                    {
                        // Add bounce to bouncer and run its media.
                        bouncer.Physics.Bounce = _bouncerBounce;
                        bouncer.RunMedia();

                        // Bouncing from a bouncing also applies
                        // acceleration to the player in the direction of the bouncer.
                        _bouncerTimer = _bouncerTimerThreshold;
                        _bouncerOrientation = bouncer.Direction;
                    }
                    else
                    {
                        // If the player isn't on top of the bouncer, disable any bounce.
                        bouncer.Physics.Bounce = 0;
                    }
                }
            }
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
            // Get important information needed in the update loop.
            // keyboard state is needed since the player's input impacts the acceleration.
            // time elapsed is needed to update timers.
            // Direction represents the vector clock-wise 90 degrees to the current orientation.
            // The direction is needed to determine acceleration due to player input.
            KeyboardState keyboardState = Keyboard.GetState();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 direction = new Vector2(
                x: -_orientationNormal.Y,
                y: _orientationNormal.X);

            // Compute the constant acceleration of the player.
            {
                // Acceleration due to gravity.
                _physics.Acceleration = _gravity;

                // If there's still time on jump enable timer, the player
                // is considered in a ground state.
                if (_jumpEnableTimer > 0)
                {
                    _jumpEnableTimer -= timeElapsed;

                    // Stick is only applied when player is on ground.
                    _physics.Stick = _stickGround;

                    // If up is pressed, immediately go into jump state.
                    // The current orientation of the player is stored as the jump
                    // orientation.
                    // The player's stick is also reset back to default to ensure
                    // the player isn't stuck to the ground when they try to jump.
                    if (keyboardState.IsKeyDown(Keys.Up))
                    {
                        if (!_jumpPressed)
                        {
                            _jumpPressed = true;
                            _jumpEnableTimer = 0;
                            _jumpTimer = _jumpTimerThreshold;
                            _jumpOrientation = _orientationNormal;
                            _physics.Stick = _stickDefault;
                        }
                    }
                    else
                        _jumpPressed = false;
                }
                // If there's no time on the jump enable timer, the player is 
                // in the air state.              
                else
                {
                    // The orientation of the player is always set to the default orientation when in air.
                    _orientationNormal = _orientationDefault;

                    // When not on ground, set to default stick.
                    _physics.Stick = _stickDefault;
                }

                // If the bouncer state is active, apply acceleration in the direction of the bouncer.
                if (_bouncerTimer > 0)
                {
                    _bouncerTimer -= timeElapsed;
                    _physics.Acceleration += _bouncerOrientation * _accelerationMagnitude * _bouncerAccelerationScale;
                }

                // Add the external acceleration. This is mainly for debugging purposes.
                _physics.Acceleration += ExternalAcceleration;

                // If jump state is active, the player can press jump to increase the player's
                // acceleration in the direction of the jump's orientation.
                if (_jumpTimer > 0)
                {
                    _jumpTimer -= timeElapsed;
                    if (keyboardState.IsKeyDown(Keys.Up))
                        _physics.Acceleration += _jumpOrientation * _accelerationMagnitude * _jumpAccelerationScale;
                }

                // If the player presses either right or left, the player is considered in a 
                // run state.
                // Acceleration is also increased along the direction vector in run state.
                if (keyboardState.IsKeyDown(Keys.Right))
                    _physics.Acceleration += direction * _accelerationMagnitude * _runAccelerationScale;
                if (keyboardState.IsKeyDown(Keys.Left))
                    _physics.Acceleration -= direction * _accelerationMagnitude * _runAccelerationScale;
            }

            // Perform the media operations, i.e. setting animations and sound effects.
            {
                // Animation is flipped based on what key is pressed.
                if (keyboardState.IsKeyDown(Keys.Right))
                    _animationCurrent.Flip = false;
                else if (keyboardState.IsKeyDown(Keys.Left))
                    _animationCurrent.Flip = true;

                // If on the ground--which is what the jump enable timer indicates--then change
                // animation and play sound effects.
                // Animations and sounds are dependent on if running, sliding, or idle.
                if (_jumpEnableTimer > 0)
                {
                    float dot = Math.Abs(Vector2.Dot(Vector2.Normalize(_physics.Velocity), direction));
                    if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.Left))
                    {
                        ChangeAnimation(animation: _animationRun, repeat: true);
                        _soundRun.Play();
                        _soundSlide.Stop();

                    }
                    else if (dot > _slideGroundThreshold)
                    {
                        ChangeAnimation(animation: _animationSlide, repeat: true);
                        _soundSlide.Play();
                    }
                    else
                    {
                        ChangeAnimation(animation: _animationIdle, repeat: true);
                        _soundSlide.Stop();
                    }                        
                }
                // If in the air, set jump and fall animations.
                else
                {
                    float dot = Vector2.Dot(_physics.Velocity, _gravity);
                    if (dot < 0)
                        ChangeAnimation(animation: _animationJump, repeat: false);
                    else
                        ChangeAnimation(animation: _animationFall, repeat: true);
                    _soundSlide.Stop();
                }

                // Set the rotation of the animation based on the orientation normal.
                _animationCurrent.Rotation = (float)Math.Atan2(y: _orientationNormal.Y, x: _orientationNormal.X) + MathHelper.PiOver2;

                // Set the position of the animation based on the position of the physics.
                _animationCurrent.Position = _physics.Position + _physics.Mask.Bounds.Center.ToVector2();
            }
#if DEBUG
           //  Console.WriteLine($"Kitty Position X: {_physics.Position.X / Wall.Width}, Y: {30 - _physics.Position.Y / Wall.Width}");
#endif
            // Run the other updates.
            base.Update(gameTime);
        }
    }
}
