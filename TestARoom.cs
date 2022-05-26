using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class TestARoom : GameObject, IDisposable
    {
        private readonly ContentManager _contentManager;
        private readonly SpriteBatch _spriteBatch;
        private readonly TestAObject _testAObject;
        private bool _disposed;
        public TestARoom(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _disposed = false;

            _testAObject = new TestAObject(
                contentManager: contentManager,
                spriteBatch: _spriteBatch,
                friction: 160f,
                accelerationMagnitude: 800f);
            PhysicsFeature testAFeature =
                _testAObject.Features
                    .Where(x => x is PhysicsFeature)
                    .Select(x => x as PhysicsFeature)
                    .Single();
            testAFeature.Position = new Vector2(x: 0, y: 0);
            Children.Add(_testAObject);

            TestBObject testBObject = new TestBObject(
                contentManager: contentManager,
                spriteBatch: _spriteBatch);
            PhysicsFeature testBFeature =
                testBObject.Features
                    .Where(x => x is PhysicsFeature)
                    .Select(x => x as PhysicsFeature)
                    .Single();
            testBFeature.Position = new Vector2(x: 300, y: 300);
            Children.Add(testBObject);

            testAFeature.CollidablePhysics.Add(testBFeature);
        }
        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // managed resources.
            if (disposing)
            {
               
                _contentManager.Unload();
            }

            // unmanaged resources.
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
