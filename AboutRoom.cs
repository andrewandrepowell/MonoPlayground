using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MonoPlayground
{
    internal class AboutRoom : GameObject
    {
        private const float _messagePositionChange = 50f;
        private readonly string[] _messageStrings;
        private readonly Vector2[] _messagePositionsInitial;
        private Vector2[] _messagePositions;
        private readonly SpriteFont _font;
        private readonly Fader _fader;
        private RoomChangeState _titleState;
        private enum RoomChangeState { Waiting, ButtonPressed, Changing };
        public bool Title { get; private set; }
        public AboutRoom(Game game)
        {
            // Text used to draw text for score and highscore.
            _font = game.Content.Load<SpriteFont>("font");

            // Fader used to fade in and out when entering and exiting room.
            _fader = new Fader(
                cameraBounds: game.GraphicsDevice.Viewport.Bounds,
                color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn();
            Children.Add(_fader);

            // Camera bounds is needed to position text.
            Rectangle cameraBounds = game.GraphicsDevice.Viewport.Bounds;

            // Messages to show in the about room.
            _messageStrings = new string[]
            {
                "This game was made by Andrew Powell.",
                "However, the following were 3rd party assets.",
                "Check the 3rd Party Asset text file in the game directory for more",
                "information.",
                " ",
                "Hit space to return to title screen.",
                " ",
                "Cat Animations",
                "URL: https://www.gameart2d.com/cat-and-dog-free-sprites.html",
                "License: CC0 1.0",
                " ",
                "Portion of the Tiles, Trees, Background, Rocks",
                "URL: https://www.gameart2d.com/free-platformer-game-tileset.html",
                "License: CC0 1.0",
                " ",
                "Montserrate Font",
                "URL: https://www.fontsquirrel.com/fonts/montserrat",
                "License: SIL OFL v1.10",
                " ",
                "Walking Sound",
                "URL: https://freesound.org/people/ProjectsU012/sounds/341025/",
                "License: CC BY 4.0",
                " ",
                "Bounce Sound",
                "URL: https://freesound.org/people/OwlStorm/sounds/404769/",
                "License: CC0 1.0",
                " ",
                "Cookie Eating Noises",
                "URL: https://freesound.org/people/OwlStorm/sounds/404781/",
                "License: CC0 1.0",
                " ",
                "Game Start/End",
                "URL: https://freesound.org/people/OwlStorm/sounds/404782/",
                "License: CC0 1.0",
                " ",
                "Game Music",
                "URL: https://freesound.org/people/Bertsz/sounds/545457/",
                "License: CC0 1.0"
            };

            // Determine starting position for messages.
            _messagePositionsInitial = _messageStrings // Messages are positioned beneath title splash.
                .Select(message => _font.MeasureString(message))
                .Select((dimen, i) => new Vector2(
                    x: MathHelper.Lerp(0, cameraBounds.Width, 0.05f),
                    y: cameraBounds.Height + i * dimen.Y))
                .ToArray();
            _messagePositions = new Vector2[_messagePositionsInitial.Length];
            _messagePositionsInitial.CopyTo(_messagePositions, 0);

            // Determines when to go back to the title room.
            _titleState = RoomChangeState.Waiting;
            Title = false;
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the position of the messages.
            if ((_messagePositions.Last().Y + _font.MeasureString(_messageStrings.Last()).Y) < 0)
                _messagePositionsInitial.CopyTo(_messagePositions, 0);
            else
                for (int i = 0; i < _messagePositions.Length; i++)
                    _messagePositions[i].Y -= timeElapsed * _messagePositionChange;

            // Determine when to go back to the title room.
            switch (_titleState)
            {
                case RoomChangeState.Waiting:
                    if (keyboardState.IsKeyDown(Keys.Space))
                        _titleState = RoomChangeState.ButtonPressed;
                    break;
                case RoomChangeState.ButtonPressed:
                    if (keyboardState.IsKeyUp(Keys.Space))
                    {
                        _fader.FadeOut();
                        _titleState = RoomChangeState.Changing;
                    }
                    break;
                case RoomChangeState.Changing:
                    if (_fader.Alpha == 1.0f)
                        Title = true;
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
                    color: Color.Black);
            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
