using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal abstract class GameObject 
    {
        public IList<GameObject> Children { get; private set; }
        public IList<GameFeature> Features { get; private set; }
        public bool Destroyed { get; private set; }
        public GameObject()
        {
            Children = new List<GameObject>();
            Features = new List<GameFeature>();
            Destroyed = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            Features.ForEach(x => x.Update(gameTime));
            Children.ForEach(x => x.Update(gameTime));
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Features.ForEach(x => x.Draw(gameTime, spriteBatch));
            Children.ForEach(x => x.Draw(gameTime, spriteBatch));
        }
        public virtual void Destroy()
        {
            if (Destroyed)
                return;
            Destroyed = true;
            Features.ForEach(x => x.Destroy());
            Children.ForEach(x => x.Destroy());
        }
    }
}
