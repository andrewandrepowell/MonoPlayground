using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
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
        private readonly Rectangle _cameraBounds;
        private string _textString;
        private Vector2 _textPosition;
        public EndRoom(Game game, int score)
        {
            _font = game.Content.Load<SpriteFont>("font");
            
            _fader = new Fader(
                cameraBounds: game.GraphicsDevice.Viewport.Bounds, 
                color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn();

            _score = score;

            _cameraBounds = game.GraphicsDevice.Viewport.Bounds;
            _textPosition = Vector2.Zero;

            Children.Add(_fader);
        }
        public override void Update(GameTime gameTime)
        {
            _textString = $"Final Score: {_score}";
            float textWidth = _textString.Select(x => _font.GetGlyphs()[x].BoundsInTexture.Width).Sum();
            float textHeight = _textString.Select(x => _font.GetGlyphs()[x].BoundsInTexture.Height).Max();
            _textPosition = new Vector2(
                x: _cameraBounds.Center.X - textWidth / 2,
                y: _cameraBounds.Center.Y - textHeight / 2);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                spriteFont: _font,
                text: _textString,
                position: _textPosition,
                color: Color.Black);
            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
