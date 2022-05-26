using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal abstract class GameFeature
    {
        private readonly GameObject _gameObject;
        public GameFeature(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        public GameObject GameObject { get => _gameObject; }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
