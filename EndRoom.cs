using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class EndRoom : GameObject
    {
        private readonly SpriteFont _font;
        private readonly Fader _fader;
        private readonly int _score;
        private readonly Vector2 _textPosition;
        public EndRoom(Game game, int score)
        {
            _font = game.Content.Load<SpriteFont>("font");
            
            _fader = new Fader(
                cameraBounds: game.GraphicsDevice.Viewport.Bounds, 
                color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn();

            _score = score;

            _textPosition = new Vector2(
                x: game.GraphicsDevice.Viewport.Bounds.Center.X,
                y: game.GraphicsDevice.Viewport.Bounds.Center.Y);

            Children.Add(_fader);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                spriteFont: _font,
                text: $"Score: {_score}",
                position: _textPosition,
                color: Color.Black);
            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
