using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class TitleRoom : GameObject
    {
        private const float _messageCoverAlphaChange = 0.1f;
        private readonly SpriteFont _font;
        private readonly Texture2D _title;
        private readonly Rectangle _cameraBounds;
        private readonly Fader _fader;
        private readonly SoundEffectInstance _soundGameStarting;
        private readonly string[] _messageStrings;
        private readonly Rectangle _messageCoverBounds;
        private StartState _gameStartState;
        private TitleState _titleState;
        private float _timer;
        private float _timerThreshold;
        private Vector2 _titlePosition;
        private Vector2[] _messagePositions;
        private Texture2D _messageCoverTexture;
        private float _messageCoverAlpha;
        
        public bool GameStarted { get; private set; }
        private enum StartState { Waiting, ButtonPressed, GameStarting };
        private enum TitleState { Waiting, MovingTitle, RevealingText };
        public TitleRoom(Game game)
        {
            _font = game.Content.Load<SpriteFont>("font");
            _title = game.Content.Load<Texture2D>("title");
            _cameraBounds = game.GraphicsDevice.Viewport.Bounds;
            _fader = new Fader(
                cameraBounds: game.GraphicsDevice.Viewport.Bounds,
                color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn();
            _soundGameStarting = game.Content.Load<SoundEffect>("endSound").CreateInstance();
            _soundGameStarting.Volume = 0.01f;
            _gameStartState = StartState.Waiting;
            _titleState = TitleState.Waiting;
            _timerThreshold = 2.5f;
            _timer = _timerThreshold;
            GameStarted = false;
            _titlePosition = new Vector2(
                x: _cameraBounds.Center.X - _title.Bounds.Center.X,
                y: _cameraBounds.Center.Y - _title.Bounds.Center.Y);
            _messageStrings = new string[]
            {
                "Welcome!",
                "Hit space to start!",
                "Use arrow keys to move Mono Kitty, collect cookies, and get to the end!"
            };
            _messagePositions = _messageStrings
                .Select(message => _font.MeasureString(message))
                .Select((dimen, i) => new Vector2(
                    x: _cameraBounds.Center.X - dimen.X / 2,
                    y: MathHelper.Lerp(_title.Bounds.Height, _cameraBounds.Height, 0.15f) + i * dimen.Y))    
                .ToArray();
            _messageCoverBounds = new Rectangle(
                location: new Point(
                    x: (int)_messagePositions.Min(pos => pos.X),
                    y: (int)_messagePositions.Min(pos => pos.Y)),
                size: new Point(
                    x: (int)_messageStrings.Select(message => _font.MeasureString(message)).Max(dimen => dimen.X),
                    y: (int)_messageStrings.Select(message => _font.MeasureString(message)).Sum(dimen => dimen.Y)));
            _messageCoverAlpha = 1.0f;
            Children.Add(_fader);
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (_gameStartState)
            {
                case StartState.Waiting:
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameStartState = StartState.ButtonPressed;
                    }
                    break;
                case StartState.ButtonPressed:
                    if (!keyboardState.IsKeyDown(Keys.Space))
                    {
                        _gameStartState = StartState.GameStarting;
                        _fader.FadeOut();
                        _soundGameStarting.Play();
                    }
                    break;
                case StartState.GameStarting:
                    if (_fader.Alpha == 1.0f)
                    {
                        GameStarted = true;
                    }
                    break;
            }
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
                    if (_messageCoverAlpha > 0.0f)
                    {
                        _messageCoverAlpha -= _messageCoverAlphaChange * timeElapsed;
                    }
                    else
                    {
                        _messageCoverAlpha = 0.0f;
                    }
                    break;
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_messageCoverTexture == null)
            {
                _messageCoverTexture = new Texture2D(
                    graphicsDevice: spriteBatch.GraphicsDevice,
                    width: _messageCoverBounds.Width,
                    height: _messageCoverBounds.Height);
                _messageCoverTexture.SetData(Enumerable
                    .Range(0, _messageCoverBounds.Width * _messageCoverBounds.Height)
                    .Select(i => Color.White)
                    .ToArray());
            }
            spriteBatch.Begin();
            _messageStrings
                .Zip(_messagePositions, (message, position) => (message, position))
                .ForEach(tuple =>
                {
                    spriteBatch.DrawString(
                        spriteFont: _font,
                        text: tuple.message,
                        position: tuple.position,
                        color: Color.Black);
                });
            spriteBatch.Draw(
                texture: _messageCoverTexture,
                destinationRectangle: _messageCoverBounds,
                color: Color.White * _messageCoverAlpha);
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
