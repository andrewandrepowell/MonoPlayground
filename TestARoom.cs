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

                wall = new Wall1(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 300, y: 300);
                walls.Add(wall);

                wall = new Wall3(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 300, y: 428);
                walls.Add(wall);

                wall = new Wall4(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 172, y: 300);
                walls.Add(wall);

                wall = new Wall5(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 172, y: 428);
                walls.Add(wall);

                wall = new Wall1(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556, y: 428);
                walls.Add(wall);

                wall = new Wall2(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 428, y: 300);
                walls.Add(wall);

                wall = new Wall7(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556+128, y: 428);
                walls.Add(wall);

                wall = new Wall8(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 128, y: 428 + 128);
                walls.Add(wall);

                wall = new Wall1(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 3*128, y: 428 + 2*128);
                walls.Add(wall);

                wall = new Wall9(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 4*128, y: 428 + 128);
                walls.Add(wall);

                wall = new Wall5(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 5 * 128, y: 428 + 0*128);
                walls.Add(wall);

                wall = new Wall4(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 5 * 128, y: 428 - 1 * 128);
                walls.Add(wall);

                wall = new Wall1(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 6 * 128, y: 428 - 1 * 128);
                walls.Add(wall);

                wall = new Wall10(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 7 * 128, y: 428 - 1 * 128);
                walls.Add(wall);

                wall = new Wall11(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 7 * 128, y: 428 - 0 * 128);
                walls.Add(wall);

                wall = new Wall12(contentManager: contentManager);
                wall.Physics.Position = new Vector2(x: 556 + 9 * 128, y: 428 + 1 * 128);
                walls.Add(wall);
            }

            _player = new MonoKitty(
                contentManager: contentManager);
            _player.Physics.Position = new Vector2(x: 500, y: -500);

            _camera = new CameraFeature(
                gameObject: this, 
                roomBounds: new Rectangle(x: 0, y:0, width: 2000, height: 2000), 
                cameraBounds: graphicsDevice.Viewport.Bounds, 
                physics: _player.Physics, 
                threshold: new Point(x:600, y: 300));
            Features.Add(_camera);

            _background = contentManager.Load<Texture2D>("scene0");

            foreach (Wall wall in walls)
            {
                Children.Add(wall);
                _player.Physics.CollidablePhysics.Add(wall.Physics);
            }

            Children.Add(_player);
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
