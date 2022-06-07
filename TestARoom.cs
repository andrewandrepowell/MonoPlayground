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
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private readonly MonoKitty _player;
        private readonly CameraFeature _camera;
        private readonly Texture2D _background;
        private bool _disposed;
        public TestARoom(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _disposed = false;

            List<Wall> walls = new List<Wall>();
            {
                Wall wall;

                wall = new WallA(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 300, y: 300);
                walls.Add(wall);

                wall = new WallB(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 300, y: 428);
                walls.Add(wall);

                wall = new WallC(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 172, y: 300);
                walls.Add(wall);

                wall = new WallD(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 172, y: 428);
                walls.Add(wall);
            }

            Wall testFWall = new Wall(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object2Mask"),
                texture: contentManager.Load<Texture2D>("object2Texture"));
            testFWall.Physics.Position = new Vector2(x: 428, y: 300);
            testFWall.Physics.Vertices.Add(new Vector2(x: 0, y: 0));
            testFWall.Physics.Vertices.Add(new Vector2(x: 15, y: 3));
            testFWall.Physics.Vertices.Add(new Vector2(x: 33, y: 17));
            testFWall.Physics.Vertices.Add(new Vector2(x: 48, y: 40));
            testFWall.Physics.Vertices.Add(new Vector2(x: 50, y: 45));
            testFWall.Physics.Vertices.Add(new Vector2(x: 63, y: 72));
            testFWall.Physics.Vertices.Add(new Vector2(x: 71, y: 87));
            testFWall.Physics.Vertices.Add(new Vector2(x: 79, y: 99));
            testFWall.Physics.Vertices.Add(new Vector2(x: 88, y: 110));
            testFWall.Physics.Vertices.Add(new Vector2(x: 118, y: 126));
            testFWall.Physics.Vertices.Add(new Vector2(x: 127, y: 127));
            Children.Add(testFWall);

            Wall testEWall = new Wall(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object3Mask"));
            testEWall.Physics.Position = new Vector2(x: 556, y: 300);
            testEWall.Physics.Vertices.Add(new Vector2(x: 0, y: 127));
            testEWall.Physics.Vertices.Add(new Vector2(x: 69, y: 121));
            testEWall.Physics.Vertices.Add(new Vector2(x: 92, y: 116));
            testEWall.Physics.Vertices.Add(new Vector2(x: 104, y: 111));
            testEWall.Physics.Vertices.Add(new Vector2(x: 114, y: 102));
            testEWall.Physics.Vertices.Add(new Vector2(x: 122, y: 90));
            testEWall.Physics.Vertices.Add(new Vector2(x: 127, y: 80));
            Children.Add(testEWall);

            Wall testAWall = new Wall(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object4Mask"));
            testAWall.Physics.Position = new Vector2(x: 684, y: 300);
            testAWall.Physics.Vertices.Add(new Vector2(x: 0, y: 77));
            testAWall.Physics.Vertices.Add(new Vector2(x: 12, y: 53));
            testAWall.Physics.Vertices.Add(new Vector2(x: 23, y: 25));
            testAWall.Physics.Vertices.Add(new Vector2(x: 30, y: 0));
            Children.Add(testAWall);

            Wall testBWall = new Wall(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object5Mask"));
            testBWall.Physics.Position = new Vector2(x: 684, y: 172);
            testBWall.Physics.Vertices.Add(new Vector2(x: 30, y: 127));
            testBWall.Physics.Vertices.Add(new Vector2(x: 42, y: 109));
            testBWall.Physics.Vertices.Add(new Vector2(x: 51, y: 91));
            testBWall.Physics.Vertices.Add(new Vector2(x: 62, y: 45));
            testBWall.Physics.Vertices.Add(new Vector2(x: 67, y: 0));
            Children.Add(testBWall);

            Wall testCWall = new Wall(
                contentManager: contentManager,
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

            _player = new MonoKitty(
                contentManager: contentManager);
            _player.Physics.Position = new Vector2(x: 500, y: -500);
            Children.Add(_player);

            _camera = new CameraFeature(
                gameObject: this, 
                roomBounds: new Rectangle(x: 0, y:0, width: 2000, height: 2000), 
                cameraBounds: graphicsDevice.Viewport.Bounds, 
                physics: _player.Physics, 
                threshold: 300);
            Features.Add(_camera);

            _background = contentManager.Load<Texture2D>("scene0");

            _player.Physics.CollidablePhysics.Add(testAWall.Physics);
            _player.Physics.CollidablePhysics.Add(testBWall.Physics);
            _player.Physics.CollidablePhysics.Add(testCWall.Physics);
            _player.Physics.CollidablePhysics.Add(testEWall.Physics);
            _player.Physics.CollidablePhysics.Add(testFWall.Physics);

            foreach (Wall wall in walls)
            {
                Children.Add(wall);
                _player.Physics.CollidablePhysics.Add(wall.Physics);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _camera.Transform);
            
            // Draw Background.
            spriteBatch.Draw(
                texture: _background, 
                destinationRectangle: new Rectangle(
                    location: _camera.Location, 
                    size: _camera.CameraBounds.Size),
                color: Color.White);

            // Draw the rest of the game objects.
            base.Draw(gameTime, spriteBatch);
            
            
            spriteBatch.End();
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
