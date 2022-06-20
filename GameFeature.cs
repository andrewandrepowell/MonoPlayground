using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal abstract class GameFeature
    {
        public GameObject GameObject { get; private set; }
        public GameFeature(GameObject gameObject)
        {
            GameObject = gameObject;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public virtual void Destroy()
        {

        }
    }
}
