using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Cookie : GameObject
    {
        private readonly static Random _random = new Random();
        private const float _floatingTimerThreshold = 0.1f;
        private const float _floatingOffsetThreshold = 3f;
        private const float _floatingOffsetChange = 1f;
        private float _floatingTimer;
        private float _floatingOffset;
        private float _floatingDirection;
        public bool Eaten { get; private set; }
        public AnimationFeature Animation { get; private set; }
        public PhysicsFeature Physics { get; private set; }
        public Cookie(ContentManager contentManager)
        {
            Physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("cookie0Mask"),
                collisionHandle: HandleCollision);
            Physics.Solid = true;
            Physics.Physics = false;
            Animation = new AnimationFeature(
                gameObject: this,
                textures: Enumerable
                    .Range(1, 11)
                    .Select(x => contentManager.Load<Texture2D>($"cookie/cookie ({x})"))
                    .ToList());
            Animation.Visible = true;
            Animation.InvisibleOnEnd = true;
            Animation.AnimationTimerThreshold = .10f;
            Eaten = false;

            _floatingTimer = _floatingTimerThreshold;
            _floatingDirection = (float)(2 * _random.Next(0, 2) - 1);
            _floatingOffset = _floatingDirection * (float)_random.NextDouble() * _floatingOffsetThreshold;

            Features.Add(Physics);
            Features.Add(Animation);
        }
        public void Eat()
        {
            if (Eaten)
                return;
            Physics.Solid = false;
            Animation.Play = true;
            Eaten = true;
        }
        private void HandleCollision(PhysicsFeature other)
        {
        }
        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If destroyed, don't run any updates.
            if (Destroyed)
                return;

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

            // If the cookie was eaten and the animation is finished, destroy the cookie.
            if (Eaten && !Animation.Play)
                Destroy();

            // Set the position of the sprite. 
            Animation.Position = Physics.Position + Physics.Mask.Bounds.Center.ToVector2() + new Vector2(x: 0, y: _floatingOffset);

            // Perform the other updatess.
            base.Update(gameTime);
        }
        public override void Destroy()
        {
            if (Destroyed)
                return;
            Animation.Destroy();
            Physics.Destroy();
            base.Destroy();
        }
    }
}
