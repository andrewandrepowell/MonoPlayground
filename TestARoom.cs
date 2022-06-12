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
                Action<Type, float, float> AddWall = delegate (Type WallType, float x, float y)
                {
                    Wall wall = (Wall)Activator.CreateInstance(WallType, contentManager);
                    wall.Physics.Position = new Vector2(x: x, y: y);
                    walls.Add(wall);
                };
                
                for (int i = 5; i < 12; i++)
                    AddWall(typeof(Wall15), Wall.Width * 0, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall14), Wall.Width * 0, _roomBounds.Height - Wall.Width * 4);
                AddWall(typeof(Wall13), Wall.Width * 1, _roomBounds.Height - Wall.Width * 4);
                for (int i = 2; i < 5; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 4);
                AddWall(typeof(Wall7), Wall.Width * 5, _roomBounds.Height - Wall.Width * 4);
                AddWall(typeof(Wall8), Wall.Width * 5, _roomBounds.Height - Wall.Width * 3);
                for (int i = 7; i < 14; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 2);
                AddWall(typeof(Wall9), Wall.Width * 14, _roomBounds.Height - Wall.Width * 3);
                for (int i = 4; i < 8; i++)
                    AddWall(typeof(Wall5), Wall.Width * 15, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall19), Wall.Width * 15, _roomBounds.Height - Wall.Width * 8);
                AddWall(typeof(Wall18), Wall.Width * 11, _roomBounds.Height - Wall.Width * 6);
                AddWall(typeof(Wall17), Wall.Width * 10, _roomBounds.Height - Wall.Width * 6);
                AddWall(typeof(Wall16), Wall.Width * 9, _roomBounds.Height - Wall.Width * 6);
                AddWall(typeof(Wall20), Wall.Width * 15, _roomBounds.Height - Wall.Width * 9);
                for (int i = 8; i < 15; i++)
                    AddWall(typeof(Wall21), Wall.Width * i, _roomBounds.Height - Wall.Width * 9);
                AddWall(typeof(Wall18), Wall.Width * 6, _roomBounds.Height - Wall.Width * 6);
                AddWall(typeof(Wall16), Wall.Width * 5, _roomBounds.Height - Wall.Width * 6);
                AddWall(typeof(Wall23), Wall.Width * 6, _roomBounds.Height - Wall.Width * 10);
                AddWall(typeof(Wall18), Wall.Width * 3, _roomBounds.Height - Wall.Width * 7);
                AddWall(typeof(Wall16), Wall.Width * 2, _roomBounds.Height - Wall.Width * 7);
                for (int i = 11; i < 14; i++)
                    AddWall(typeof(Wall5), Wall.Width * 7, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall24), Wall.Width * 2, _roomBounds.Height - Wall.Width * 13);
                AddWall(typeof(Wall21), Wall.Width * 1, _roomBounds.Height - Wall.Width * 12);
                AddWall(typeof(Wall25), Wall.Width * 0, _roomBounds.Height - Wall.Width * 12);                
            }
            
            List<Bouncer> bouncers = new List<Bouncer>();
            {
                Action<Type, float, float, float, float> AddBouncer = delegate(Type BouncerType, float posx, float posy, float dirx, float diry)
                {
                    Bouncer bouncer = (Bouncer)Activator.CreateInstance(BouncerType, contentManager);
                    bouncer.Physics.Position = new Vector2(x: posx, y: posy);
                    bouncer.Direction = new Vector2(x: dirx, y: diry);
                    bouncers.Add(bouncer);
                };

                AddBouncer(typeof(Bouncer), Wall.Width * 1, Wall.Width * 22, 1, -1);
            }

            _player = new MonoKitty(
                contentManager: contentManager);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 2,
            //    y: _roomBounds.Height - Wall.Width * 6);
            _player.Physics.Position = new Vector2(
                x: Wall.Width * 3,
                y: Wall.Width * 22);

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

            foreach (Bouncer bouncer in bouncers)
            {
                Children.Add(bouncer);
                _player.Physics.CollidablePhysics.Add(bouncer.Physics);
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
