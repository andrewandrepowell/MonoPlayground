using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;

namespace MonoPlayground
{
    internal class Fader : GameObject
    {
        private const float _fadeChangeAmount = 0.75f;
        private Texture2D _texture;
        private readonly Color _color;
        private readonly Rectangle _cameraBounds;
        private float _fadeChangeDirection = 0.0f;
        public float Alpha { get; set; }
        public Point Position { get; set; }
        public Fader(Rectangle cameraBounds, Color color)
        {
            _color = color;
            _cameraBounds = cameraBounds;
            Position = Point.Zero;
            Alpha = 0.0f;
            _fadeChangeDirection = 0.0f;
        }
        public void FadeIn() => _fadeChangeDirection = -_fadeChangeAmount;
        public void FadeOut() => _fadeChangeDirection = +_fadeChangeAmount;
        public override void Update(GameTime gameTime)
        {
            if (Alpha < 0.0f || Alpha > 1.0f)
                throw new ArgumentOutOfRangeException("Alpha must be between 0.0f and 1.0f inclusively.");
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Alpha += _fadeChangeDirection * timeElapsed;
            if (Alpha < 0.0f)
                Alpha = 0.0f;
            else if (Alpha > 1.0f)
                Alpha = 1.0f;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(
                    graphicsDevice: spriteBatch.GraphicsDevice,
                    width: _cameraBounds.Width,
                    height: _cameraBounds.Height);
                _texture.SetData(Enumerable
                    .Range(0, _cameraBounds.Width * _cameraBounds.Height)
                    .Select(i => _color)
                    .ToArray());
            }
            spriteBatch.Draw(
                texture: _texture,
                destinationRectangle: new Rectangle(
                    location: Position,
                    size: _cameraBounds.Size),
                color: _color * Alpha);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
