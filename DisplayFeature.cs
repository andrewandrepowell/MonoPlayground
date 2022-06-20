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
        private Vector2 _position;
        public DisplayFeature(GameObject gameObject, Texture2D texture) : base(gameObject)
        {
            _texture = texture;
            _position = Vector2.Zero;
        }
        public Vector2 Position { get => _position; set => _position = value; }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: _texture,
                destinationRectangle: new Rectangle(
                    location: _position.ToPoint(), 
                    size: _texture.Bounds.Size), 
                color: Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
