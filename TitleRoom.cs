using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class TitleRoom : GameObject
    {
        private readonly SpriteFont _font;
        private readonly Texture2D _title;
        private readonly Rectangle _cameraBounds;
        private readonly Fader _fader;
        private readonly SoundEffectInstance _soundGameStarting;
        private StartState _gameStartState;
        public bool GameStarted { get; private set; }
        private enum StartState { Waiting, ButtonPressed, GameStarting };
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
            GameStarted = false;
            Children.Add(_fader);
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
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
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                texture: _title,
                destinationRectangle: new Rectangle(
                    location: new Point(
                        x: _cameraBounds.Center.X - _title.Bounds.Center.X, 
                        y: _cameraBounds.Center.Y - _title.Bounds.Center.Y),
                    size: _title.Bounds.Size),
                color: Color.White);
            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
