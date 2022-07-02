using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Tree : GameObject
    {
        private const int _treeTotal = 2;
        private static Texture2D[] _textures;
        private static readonly Random _random = new Random();
        private Texture2D _texture;
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; private set; }
        public Tree(Game game)
        {
            if (_textures == null)
            {
                _textures = Enumerable
                    .Range(1, _treeTotal)
                    .Select(i => game.Content.Load<Texture2D>($"tree/tree ({i})"))
                    .ToArray();
            }
            _texture = _textures[_random.Next(0, _treeTotal)];
            Bounds = new Rectangle(
                x: 0,
                y: 0,
                width: (int)Math.Ceiling((double)_textures.Select(x => x.Bounds.Width).Max() / Wall.Width) * Wall.Width,
                height: (int)Math.Ceiling((double)_textures.Select(x => x.Bounds.Height).Max() / Wall.Width) * Wall.Width);
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
