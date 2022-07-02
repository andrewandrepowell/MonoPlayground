using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoPlayground
{
    internal class Bouncer : GameObject
    {
        private static readonly Random _random = new Random();
        private const int _verticesPerMask = 32;
        private const float _floatingTimerThreshold = 0.2f;
        private const float _floatingOffsetThreshold = 3f;
        private const float _floatingOffsetChange = 1f;
        private Vector2 _direction;
        private float _floatingTimer;
        private float _floatingOffset;
        private float _floatingDirection;
        public PhysicsFeature Physics { get; private set; }
        public AnimationFeature Animation { get; private set; }
        public SoundEffectInstance SoundEffect { get; private set; }
        public Vector2 Direction
        {
            get => _direction;
            set => _direction = Vector2.Normalize(value);
        }
        public Bouncer(ContentManager contentManager)
        {
            Physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("bouncer0Mask"),
                collisionHandle: HandleCollision);
            Physics.Physics = false;
            Physics.Solid = true;
            Physics.Bounce = 0;
            Enumerable.Range(0, _verticesPerMask)
                .Select((i) => (
                    x: (Math.Cos(MathHelper.TwoPi * i / _verticesPerMask - MathHelper.Pi) + 1) * Physics.Mask.Width / 2, 
                    y: (Math.Sin(MathHelper.TwoPi * i / _verticesPerMask - MathHelper.Pi) + 1) * Physics.Mask.Height / 2))
                .ForEach(pair => Physics.Vertices.Add(new Vector2(
                    x: (float)pair.x, 
                    y: (float)pair.y)));
            Features.Add(Physics);

            Animation = new AnimationFeature(
                gameObject: this,
                textures: Enumerable
                    .Range(0, 11)
                    .Select( i => contentManager.Load<Texture2D>($"bouncer/bouncer ({i})") )
                    .ToList());
            Animation.Visible = true;
            Features.Add(Animation);

            SoundEffect = contentManager.Load<SoundEffect>("bounceSound").CreateInstance();
            SoundEffect.Volume = 0.01f;

            Direction = -Vector2.UnitY;

            _floatingTimer = _floatingTimerThreshold;
            _floatingDirection = (float)(2 * _random.Next(0, 2) - 1);
            _floatingOffset = _floatingDirection * (float)_random.NextDouble() * _floatingOffsetThreshold;
        }
        public void RunMedia()
        {
            Animation.Reset();
            Animation.Visible = true;
            Animation.Play = true;
            SoundEffect.Play();
        }
        private void HandleCollision(PhysicsFeature other)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Perform calculations related to the bouncer floating.
            if (_floatingTimer > 0)
            {
                _floatingTimer -= timeElapsed;
            }
            else
            {
                _floatingTimer = _floatingTimerThreshold;
                _floatingOffset += _floatingDirection * _floatingOffsetChange;
                if (_floatingOffset > _floatingOffsetThreshold)
                {
                    _floatingDirection = -1f;
                }
                else if (_floatingOffset < -_floatingOffsetThreshold)
                {
                    _floatingDirection = 1f;
                }
            }
            
            // Update the animation rotation and position.
            Animation.Rotation = (float)Math.Atan2(y: _direction.Y, x: _direction.X) + MathHelper.PiOver2;
            Animation.Position = Physics.Position + Physics.Mask.Bounds.Center.ToVector2() + new Vector2(x: 0, y: _floatingOffset);

            // Perform the reset of the updates.
            base.Update(gameTime);
        }
    }
}
