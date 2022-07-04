using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MonoPlayground
{
    internal class EndRoom : GameObject
    {
        private static readonly string _highscoreFile = "save.kitty";
        private readonly SpriteFont _font;
        private readonly Fader _fader;
        private readonly bool _highscoreNew;
        private readonly string _scoreTextString;
        private readonly Vector2 _scoreTextPosition;
        private readonly string _highscoreTextString;
        private readonly Vector2 _highscoreTextPosition;
        private readonly string _messageTextString;
        private readonly Vector2 _messageTextPosition;
        public EndRoom(Game game, int score)
        {
            // Text used to draw text for score and highscore.
            _font = game.Content.Load<SpriteFont>("font");

            // Fader used to fade in and out when entering and exiting room.
            _fader = new Fader(
                cameraBounds: game.GraphicsDevice.Viewport.Bounds, 
                color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn();

            // Camera bounds is needed to position text.
            Rectangle cameraBounds = game.GraphicsDevice.Viewport.Bounds;
            _scoreTextPosition = Vector2.Zero;

            // Update the high score if new high score occurs.
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

            // Generate the text to display and their positions on the screen.
            
            _scoreTextString = $"Final Score: {score}";
            _highscoreTextString = $"High Score: {highscore}";
            _messageTextString = _highscoreNew ? "New High Score! Thanks for playing!" : "Thanks for playing!";

            Vector2 _scoreTextSize = _font.MeasureString(_scoreTextString);
            Vector2 _highscoreTextSize = _font.MeasureString(_highscoreTextString);
            Vector2 _messageTextSize = _font.MeasureString(_messageTextString);
            float _totalTextHeight = _scoreTextSize.Y + _highscoreTextSize.Y + _messageTextSize.Y;

            _scoreTextPosition = new Vector2(
                x: cameraBounds.Center.X - _scoreTextSize.X / 2,
                y: cameraBounds.Center.Y - _totalTextHeight / 2);
            _highscoreTextPosition = new Vector2(
                x: cameraBounds.Center.X - _highscoreTextSize.X / 2,
                y: cameraBounds.Center.Y - _totalTextHeight / 2 + _scoreTextSize.Y);
            _messageTextPosition = new Vector2(
                x: cameraBounds.Center.X - _messageTextSize.X / 2,
                y: cameraBounds.Center.Y - _totalTextHeight / 2 + _scoreTextSize.Y + _highscoreTextSize.Y);

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
