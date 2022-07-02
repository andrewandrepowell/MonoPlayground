using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class AboutRoom : GameObject
    {
        private readonly string[] _messageStrings;
        private readonly Vector2 _messagePositions;
        private readonly SpriteFont _font;
        private readonly Fader _fader;

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

            // Camera bounds is needed to position text.
            Rectangle cameraBounds = game.GraphicsDevice.Viewport.Bounds;

            _messageStrings = new string[]
            {
                "This game was made by Andrew Powell.",
                "However, the following were 3rd Party assets.",
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

            _messagePositions = _messageStrings // Messages are positioned beneath title splash.
                .Select(message => _font.MeasureString(message))
                .Select((dimen, i) => new Vector2(
                    x: cameraBounds.Center.X - dimen.X / 2,
                    y: MathHelper.Lerp(_title.Bounds.Height, cameraBounds.Height, 0.15f) + i * dimen.Y))
                .ToArray();
        }
    }
}
