using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoPlayground
{
    internal class Bouncer : GameObject
    {
        private const int _verticesPerMask = 32;
        private Vector2 _direction;
        public PhysicsFeature Physics { get; private set; }
        public AnimationFeature Animation { get; private set; }
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

            Animation = new AnimationFeature(
                gameObject: this,
                textures: Enumerable
                    .Range(0, 11)
                    .Select( i => contentManager.Load<Texture2D>($"bouncer/bouncer ({i})") )
                    .ToList());
            Animation.Visible = true;

            Direction = -Vector2.UnitY;

            Features.Add(Physics);
            Features.Add(Animation);
        }
        public void RunMedia()
        {
            Animation.Reset();
            Animation.Visible = true;
            Animation.Play = true;
        }
        private void HandleCollision(PhysicsFeature other)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            Animation.Rotation = (float)Math.Atan2(y: _direction.Y, x: _direction.X) + MathHelper.PiOver2;
            Animation.Position = Physics.Position + Physics.Mask.Bounds.Center.ToVector2();
            base.Update(gameTime);
        }
    }
}
