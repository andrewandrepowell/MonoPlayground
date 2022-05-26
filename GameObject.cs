using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal abstract class GameObject
    {
        private readonly ICollection<GameObject> _children;
        private readonly ICollection<GameFeature> _features;
        public GameObject()
        {
            _children = new List<GameObject>();
            _features = new List<GameFeature>();
        }
        public ICollection<GameObject> Children { get => _children; }
        public ICollection<GameFeature> Features { get => _features;  }
        public virtual void Update(GameTime gameTime)
        {
            _features.ForEach(x => x.Update(gameTime));
            _children.ForEach(x => x.Update(gameTime));
        }
        public virtual void Draw(GameTime gameTime)
        {
            _features.ForEach(x => x.Draw(gameTime));
            _children.ForEach(x => x.Draw(gameTime));
        }
    }
}
