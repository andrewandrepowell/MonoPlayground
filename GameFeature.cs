using System;
using Microsoft.Xna.Framework;

namespace MonoPlayground
{
    internal abstract class GameFeature : IDisposable
    {
        private readonly GameObject _gameObject;
        private bool _disposed;
        public GameFeature(GameObject gameObject)
        {
            _gameObject = gameObject;
            _disposed = false;
        }
        public GameObject GameObject { get => _gameObject; }
        public abstract void Update(GameTime gameTime);
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
