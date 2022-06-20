using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Scoreboard : GameObject
    {
        private static readonly Vector2 _textOffset = new Vector2(x: 35, y: 20); 
        private readonly SpriteFont _font;
        private readonly Texture2D _texture;
        public int Score { get; set; }
        public Vector2 Position { get; set; }
        public Scoreboard(ContentManager contentManager)
        {
            _font = contentManager.Load<SpriteFont>("font");
            _texture = contentManager.Load<Texture2D>("score0");
            Score = 0;
            Position = new Vector2(x: 0, y: 0);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: _texture,
                destinationRectangle: new Rectangle(
                    location: Position.ToPoint(),
                    size: _texture.Bounds.Size),
                color: Color.White);
            spriteBatch.DrawString(
                spriteFont: _font, 
                text: $"Score: {Score}", 
                position: Position + _textOffset, 
                color: Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
