using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoPlayground
{
    internal abstract class GameObject : IDisposable
    {
        private readonly ICollection<GameObject> _children;
        private readonly ICollection<GameFeature> _features;
        private bool _disposed;
        public GameObject()
        {
            _children = new List<GameObject>();
            _features = new List<GameFeature>();
            _disposed = false;
        }
        public ICollection<GameObject> Children { get => _children; }
        public ICollection<GameFeature> Features { get => _features;  }
        public void Update(GameTime gameTime)
        {
            _features.ToList().ForEach(x => x.Update(gameTime));
            _children.ToList().ForEach(x => x.Update(gameTime));
        }
        public void Dispose()
        {
            Dispose(disposing: true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _children.ToList().ForEach(x => x.Dispose());
                _features.ToList().ForEach(x => x.Dispose());
            }

            _disposed = true;
        }
    }
}
