using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class EndRoom : GameObject
    {
        private static readonly string _highscoreFile = "save.kitty";
        private readonly SpriteFont _font;
        private readonly Fader _fader;
        private readonly Rectangle _cameraBounds;
        private readonly bool _highscoreNew;
        private readonly string _scoreTextString;
        private readonly Vector2 _scoreTextPosition;
        private readonly string _highscoreTextString;
        private readonly Vector2 _highscoreTextPosition;
        private readonly string _messageTextString;
        private readonly Vector2 _messageTextPosition;
        public EndRoom(Game game, int score)
        {
            _font = game.Content.Load<SpriteFont>("font");
            
            _fader = new Fader(
                cameraBounds: game.GraphicsDevice.Viewport.Bounds, 
                color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn();

            _cameraBounds = game.GraphicsDevice.Viewport.Bounds;
            _scoreTextPosition = Vector2.Zero;

            int highscore = 0;
            try
            {
                highscore = Saver.Load<int>(_highscoreFile);
            }
            catch (FileNotFoundException)
            {
                highscore = 0;
            }
            if (score > highscore)
            {
                _highscoreNew = true;
                highscore = score;
                Saver.Save(_highscoreFile, score);
            }
            else
            {
                _highscoreNew = false;
            }

            _scoreTextString = $"Final Score: {score}";
            _highscoreTextString = $"High Score: {highscore}";
            _messageTextString = _highscoreNew ? "New High Score! Thanks for playing!" : "Thanks for playing!";

            Vector2 _scoreTextSize = _font.MeasureString(_scoreTextString);
            Vector2 _highscoreTextSize = _font.MeasureString(_highscoreTextString);
            Vector2 _messageTextSize = _font.MeasureString(_messageTextString);
            float _totalTextHeight = _scoreTextSize.Y + _highscoreTextSize.Y + _messageTextSize.Y;

            _scoreTextPosition = new Vector2(
                x: _cameraBounds.Center.X - _scoreTextSize.X / 2,
                y: _cameraBounds.Center.Y - _totalTextHeight / 2);
            _highscoreTextPosition = new Vector2(
                x: _cameraBounds.Center.X - _highscoreTextSize.X / 2,
                y: _cameraBounds.Center.Y - _totalTextHeight / 2 + _scoreTextSize.Y);
            _messageTextPosition = new Vector2(
                x: _cameraBounds.Center.X - _messageTextSize.X / 2,
                y: _cameraBounds.Center.Y - _totalTextHeight / 2 + _scoreTextSize.Y + _highscoreTextSize.Y);

            Children.Add(_fader);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                spriteFont: _font,
                text: _scoreTextString,
                position: _scoreTextPosition,
                color: Color.Black);
            spriteBatch.DrawString(
                spriteFont: _font,
                text: _highscoreTextString,
                position: _highscoreTextPosition,
                color: Color.Black);
            spriteBatch.DrawString(
                spriteFont: _font,
                text: _messageTextString,
                position: _messageTextPosition,
                color: Color.Black);
            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
