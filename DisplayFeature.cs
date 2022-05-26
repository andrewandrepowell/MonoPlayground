using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class DisplayFeature : GameFeature
    {
        private readonly Texture2D _texture;
        private readonly SpriteBatch _spriteBatch;
        private Vector2 _position;
        public DisplayFeature(GameObject gameObject, Texture2D texture, SpriteBatch spriteBatch) : base(gameObject)
        {
            _texture = texture;
            _spriteBatch = spriteBatch;
            _position = Vector2.Zero;
        }
        public Vector2 Position { get => _position; set => _position = value; }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture: _texture, position: _position, color: Color.White);
            _spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            return;
        }
    }
}
