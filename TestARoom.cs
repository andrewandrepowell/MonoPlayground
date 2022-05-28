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
            _testAObject.Physics.Position = new Vector2(x: 0, y: 0);
            Children.Add(_testAObject);

            TestBObject testBObject = new TestBObject(
                contentManager: contentManager,
                spriteBatch: _spriteBatch);
            testBObject.Physics.Position = new Vector2(x: 300, y: 300);
            Children.Add(testBObject);

            TestCObject testCObject = new TestCObject(
                contentManager: contentManager,
                spriteBatch: _spriteBatch);
            testCObject.Physics.Position = new Vector2(x: 428, y: 300);
            Children.Add(testCObject);

            TestDObject testDObject = new TestDObject(
                contentManager: contentManager,
                spriteBatch: _spriteBatch);
            testDObject.Physics.Position = new Vector2(x: 556, y: 300);
            Children.Add(testDObject);

            _testAObject.Physics.CollidablePhysics.Add(testBObject.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testCObject.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testDObject.Physics);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.DrawLine(
                point1: _testAObject.Physics.CollisionPoint + _testAObject.Physics.CollisionNormal * 200,
                point2: _testAObject.Physics.CollisionPoint,
                color: Color.Red,
                thickness: 2);
            _spriteBatch.End();
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
