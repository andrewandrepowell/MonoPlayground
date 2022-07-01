using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Bush : GameObject
    {
        private const int _bushTotal = 4;
        private static readonly Random _random = new Random();
        private static Texture2D[] _textures;
        private Texture2D _texture;
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get => new Rectangle(x:0, y:0, width:Wall.Width, height:Wall.Width); }
        public Bush(Game game)
        {
            if (_textures == null)
            {
                _textures = Enumerable.Range(1, _bushTotal)
                    .Select(i => game.Content.Load<Texture2D>($"bush/bush ({i})"))
                    .ToArray();
            }
            _texture = _textures[_random.Next(0, _bushTotal)];
            Position = Vector2.Zero;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: _texture,
                position: new Vector2(
                    x: Position.X + Bounds.Center.X - _texture.Bounds.Center.X,
                    y: Position.Y + Bounds.Height - _texture.Bounds.Height),
                color: Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
