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
        private static readonly Rectangle _roomBounds = new Rectangle(
            x: 0, y: 0, 
            width: Wall.Width * 40, 
            height: Wall.Width * 30);
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

                for (int i = 5; i * Wall.Width <= _roomBounds.Height; i++)
                {
                    wall = new Wall15(contentManager: contentManager);
                    wall.Physics.Position = new Vector2(
                        x: Wall.Width * 0,
                        y: _roomBounds.Height - Wall.Width * i);
                    walls.Add(wall);
                }

                wall = new Wall14(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 0,
                    y: _roomBounds.Height - Wall.Width * 4);
                walls.Add(wall);
                
                wall = new Wall13(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 1,
                    y: _roomBounds.Height - Wall.Width * 4);
                walls.Add(wall);

                for (int i = 2; i < 5; i++)
                {
                    wall = new Wall1(contentManager: contentManager);
                    wall.Physics.Position = new Vector2(
                        x: Wall.Width * i,
                        y: _roomBounds.Height - Wall.Width * 4);
                    walls.Add(wall);
                }

                wall = new Wall7(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 5,
                    y: _roomBounds.Height - Wall.Width * 4);
                walls.Add(wall);

                wall = new Wall8(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 5,
                    y: _roomBounds.Height - Wall.Width * 3);
                walls.Add(wall);

                for (int i = 7; i < 10; i++)
                {
                    wall = new Wall1(contentManager: contentManager);
                    wall.Physics.Position = new Vector2(
                        x: Wall.Width * i,
                        y: _roomBounds.Height - Wall.Width * 2);
                    walls.Add(wall);
                }

                wall = new Wall9(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 10,
                    y: _roomBounds.Height - Wall.Width * 3);
                walls.Add(wall);

                for (int i = 4; i < 8; i++)
                {
                    wall = new Wall5(contentManager: contentManager);
                    wall.Physics.Position = new Vector2(
                        x: Wall.Width * 11,
                        y: _roomBounds.Height - Wall.Width * i);
                    walls.Add(wall);
                }

                wall = new Wall19(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 11,
                    y: _roomBounds.Height - Wall.Width * 8);
                walls.Add(wall);

                wall = new Wall18(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 8,
                    y: _roomBounds.Height - Wall.Width * 6);
                walls.Add(wall);

                wall = new Wall17(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 7,
                    y: _roomBounds.Height - Wall.Width * 6);
                walls.Add(wall);

                wall = new Wall16(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 6,
                    y: _roomBounds.Height - Wall.Width * 6);
                walls.Add(wall);

                wall = new Wall20(contentManager: contentManager);
                wall.Physics.Position = new Vector2(
                    x: Wall.Width * 11,
                    y: _roomBounds.Height - Wall.Width * 9);
                walls.Add(wall);

                for (int i = 7; i < 11; i++)
                {
                    wall = new Wall21(contentManager: contentManager);
                    wall.Physics.Position = new Vector2(
                        x: Wall.Width * i,
                        y: _roomBounds.Height - Wall.Width * 9);
                    walls.Add(wall);
                }
            }

            _player = new MonoKitty(
                contentManager: contentManager);
            _player.Physics.Position = new Vector2(
                        x: Wall.Width * 2,
                        y: _roomBounds.Height - Wall.Width * 6);

            _camera = new CameraFeature(
                gameObject: this, 
                roomBounds: _roomBounds, 
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
