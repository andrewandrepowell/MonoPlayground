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
                mask: "object0Mask",
                friction: 600f,
                accelerationMagnitude: 1000f,
                maxSpeed: 400,
                bounce: .1f,
                gravity: new Vector2(x:0, y: 2000));
            _testAObject.Physics.Position = new Vector2(x: 400, y: 0);
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

            TestGeneralWall testAWall = new TestGeneralWall(
                contentManager: contentManager,
                spriteBatch: _spriteBatch,
                mask: contentManager.Load<Texture2D>("object4Mask"));
            testAWall.Physics.Position = new Vector2(x: 684, y: 300);
            testAWall.Physics.Vertices.Add(new Vector2(x: 0, y: 77));
            testAWall.Physics.Vertices.Add(new Vector2(x: 12, y: 53));
            testAWall.Physics.Vertices.Add(new Vector2(x: 23, y: 25));
            testAWall.Physics.Vertices.Add(new Vector2(x: 30, y: 0));
            Children.Add(testAWall);

            TestGeneralWall testBWall = new TestGeneralWall(
                contentManager: contentManager,
                spriteBatch: _spriteBatch,
                mask: contentManager.Load<Texture2D>("object5Mask"));
            testBWall.Physics.Position = new Vector2(x: 684, y: 172);
            testBWall.Physics.Vertices.Add(new Vector2(x: 30, y: 127));
            testBWall.Physics.Vertices.Add(new Vector2(x: 42, y: 109));
            testBWall.Physics.Vertices.Add(new Vector2(x: 51, y: 91));
            testBWall.Physics.Vertices.Add(new Vector2(x: 62, y: 45));
            testBWall.Physics.Vertices.Add(new Vector2(x: 67, y: 0));
            Children.Add(testBWall);

            TestGeneralWall testCWall = new TestGeneralWall(
                contentManager: contentManager,
                spriteBatch: _spriteBatch,
                mask: contentManager.Load<Texture2D>("object6Mask"));
            testCWall.Physics.Position = new Vector2(x: 556, y: 44);
            testCWall.Physics.Vertices.Add(new Vector2(x: 195, y: 127));
            testCWall.Physics.Vertices.Add(new Vector2(x: 193, y: 89));
            testCWall.Physics.Vertices.Add(new Vector2(x: 190, y: 70));
            testCWall.Physics.Vertices.Add(new Vector2(x: 184, y: 55));
            testCWall.Physics.Vertices.Add(new Vector2(x: 173, y: 42));
            testCWall.Physics.Vertices.Add(new Vector2(x: 152, y: 30));
            testCWall.Physics.Vertices.Add(new Vector2(x: 120, y: 17));
            testCWall.Physics.Vertices.Add(new Vector2(x: 97, y: 9));
            testCWall.Physics.Vertices.Add(new Vector2(x: 69, y: 0));
            Children.Add(testCWall);

            TestGeneralWall testDWall = new TestGeneralWall(
                contentManager: contentManager,
                spriteBatch: _spriteBatch,
                mask: contentManager.Load<Texture2D>("object1Mask"));
            testDWall.Physics.Position = new Vector2(x: 172, y: 172);
            testDWall.Physics.Vertices.Add(new Vector2(x: 0, y: 0));
            testDWall.Physics.Vertices.Add(new Vector2(x: 127, y: 0));
            testDWall.Physics.Vertices.Add(new Vector2(x: 127, y: 127));
            Children.Add(testDWall);

            _testAObject.Physics.CollidablePhysics.Add(testBObject.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testCObject.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testDObject.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testAWall.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testBWall.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testCWall.Physics);
            _testAObject.Physics.CollidablePhysics.Add(testDWall.Physics);
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
