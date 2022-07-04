using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace MonoPlayground
{
    internal class TitleRoom : GameObject
    {
        private const float _messageAlphaChange = 0.2f;
        private readonly SpriteFont _font;
        private readonly Texture2D _title;
        private readonly Fader _fader;
        private readonly SoundEffectInstance _soundGameStarting;
        private readonly string[] _messageStrings;
        private RoomChangeState _gameStartState;
        private RoomChangeState _aboutState;
        private TitleState _titleState;
        private float _timer;
        private float _timerThreshold;
        private Vector2 _titlePosition;
        private Vector2[] _messagePositions;
        private float _messageAlpha;
        public bool GameStarted { get; private set; }
        public bool About { get; private set; }
        private enum RoomChangeState { Waiting, ButtonPressed, Changing };
        private enum TitleState { Waiting, MovingTitle, RevealingText };
        public TitleRoom(Game game)
        {
            _font = game.Content.Load<SpriteFont>("font"); // Font used to draw up the text.
            _title = game.Content.Load<Texture2D>("title"); // Splash screen.
            Rectangle cameraBounds = game.GraphicsDevice.Viewport.Bounds; // Bounds of the window is needed.
            _fader = new Fader( // Fader is needed to fade in and out when entering and exiting room.
                cameraBounds: cameraBounds,
                color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn(); // Fade in at start.
            _soundGameStarting = game.Content.Load<SoundEffect>("endSound").CreateInstance(); 
            _soundGameStarting.Volume = 0.01f;
            _gameStartState = RoomChangeState.Waiting; // FSM used to determine how the game starts.
            _aboutState = RoomChangeState.Waiting; // FSM used to determine going to the about room.
            _titleState = TitleState.Waiting; // FSM used to determine title screen animation.
            _timerThreshold = 2.5f; // Multi-purpose timer used in FSM.
            _timer = _timerThreshold;
            GameStarted = false;
            About = false;
            _titlePosition = new Vector2( // Title starts in middle on screen.
                x: cameraBounds.Center.X - _title.Bounds.Center.X,
                y: cameraBounds.Center.Y - _title.Bounds.Center.Y);
            _messageStrings = new string[] // Messages to show during title animation.
            {
                "Welcome!",
                "Hit space to start! (or \'A\' to go to the about page)",
                "Use arrow keys to move Mono Kitty, collect cookies, and get to the end!"
            };
            _messagePositions = _messageStrings // Messages are positioned beneath title splash.
                .Select(message => _font.MeasureString(message))
                .Select((dimen, i) => new Vector2(
                    x: cameraBounds.Center.X - dimen.X / 2,
                    y: MathHelper.Lerp(_title.Bounds.Height, cameraBounds.Height, 0.15f) + i * dimen.Y))    
                .ToArray();
            _messageAlpha = 0.0f; // Messages are initially invisible.
            Children.Add(_fader); // Fader is added to children.
        }
        public override void Update(GameTime gameTime)
        {
            // Get keyboard state and time elapsed is needed.
            KeyboardState keyboardState = Keyboard.GetState();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // FSM used to determine how the game starts.
            switch (_gameStartState)
            {
                case RoomChangeState.Waiting:
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameStartState = RoomChangeState.ButtonPressed;
                    }
                    break;
                case RoomChangeState.ButtonPressed:
                    if (!keyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameStartState = RoomChangeState.Changing;
                        _fader.FadeOut();
                        _soundGameStarting.Play();
                    }
                    break;
                case RoomChangeState.Changing:
                    if (_fader.Alpha == 1.0f)
                    {
                        GameStarted = true;
                    }
                    break;
            }

            // FSM used to determine going to the about room.
            switch (_aboutState)
            {
                case RoomChangeState.Waiting:
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        _aboutState = RoomChangeState.ButtonPressed;
                    }
                    break;
                case RoomChangeState.ButtonPressed:
                    if (!keyboardState.IsKeyDown(Keys.A))
                    {
                        _aboutState = RoomChangeState.Changing;
                        _fader.FadeOut();
                    }
                    break;
                case RoomChangeState.Changing:
                    if (_fader.Alpha == 1.0f)
                    {
                        About = true;
                    }
                    break;
            }

            // FSM used to determine title screen animation.
            switch (_titleState)
            {
                case TitleState.Waiting:
                    if (_timer > 0.0f)
                        _timer -= timeElapsed;
                    else
                        _titleState = TitleState.MovingTitle;
                    break;
                case TitleState.MovingTitle:
                    if (_titlePosition.Y > 0.0f)
                        _titlePosition.Y -= 100*timeElapsed;
                    else
                    {
                        _titlePosition.Y = 0;
                        _titleState = TitleState.RevealingText;
                    }
                    break;
                case TitleState.RevealingText:
                    if (_messageAlpha < 1.0f)
                    {
                        _messageAlpha += _messageAlphaChange * timeElapsed;
                    }
                    else
                    {
                        _messageAlpha = 1.0f;
                    }
                    break;
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach ((string message, Vector2 position) in _messageStrings.Zip(_messagePositions, (message, position) => (message, position)))
                spriteBatch.DrawString(
                    spriteFont: _font,
                    text: message,
                    position: position,
                    color: Color.Black * _messageAlpha);
            spriteBatch.Draw(
                texture: _title,
                destinationRectangle: new Rectangle(
                    location: _titlePosition.ToPoint(),
                    size: _title.Bounds.Size),
                color: Color.White);
            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
