using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
                AddWall(typeof(Wall19), Wall.Width * 7, _roomBounds.Height - Wall.Width * 14);
                AddWall(typeof(Wall20), Wall.Width * 7, _roomBounds.Height - Wall.Width * 15);
                for (int i = 5; i < 7; i++)
                    AddWall(typeof(Wall21), Wall.Width * i, _roomBounds.Height - Wall.Width * 15);
                AddWall(typeof(Wall27), Wall.Width * 4, _roomBounds.Height - Wall.Width * 15);
                for (int i = 16; i < 17; i++)
                    AddWall(typeof(Wall28), Wall.Width * 4, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall12), Wall.Width * 4, _roomBounds.Height - Wall.Width * 17);
                AddWall(typeof(Wall24), Wall.Width * 2, _roomBounds.Height - Wall.Width * 13);
                AddWall(typeof(Wall21), Wall.Width * 1, _roomBounds.Height - Wall.Width * 12);
                AddWall(typeof(Wall36), Wall.Width * 0, _roomBounds.Height - Wall.Width * 12);
                for (int i = 14; i < 18; i++)
                    AddWall(typeof(Wall26), Wall.Width * 2, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall29), Wall.Width * 2, _roomBounds.Height - Wall.Width * 18);
                AddWall(typeof(Wall25), Wall.Width * 2, _roomBounds.Height - Wall.Width * 19);
                for (int i = 3; i < 8; i++)
                    AddWall(typeof(Wall21), Wall.Width * i, _roomBounds.Height - Wall.Width * 19);
                AddWall(typeof(Wall11), Wall.Width * 8, _roomBounds.Height - Wall.Width * 19);
                AddWall(typeof(Wall10), Wall.Width * 8, _roomBounds.Height - Wall.Width * 20);
                AddWall(typeof(Wall30), Wall.Width * 6, _roomBounds.Height - Wall.Width * 21);
                for (int i = 4; i < 6; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 21);
                AddWall(typeof(Wall8), Wall.Width * 2, _roomBounds.Height - Wall.Width * 22);
                for (int i = 23; i < 27; i++)
                    AddWall(typeof(Wall26), Wall.Width * 2, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall33), Wall.Width * 2, _roomBounds.Height - Wall.Width * 28);
                for (int i = 4; i < 7; i++)
                    AddWall(typeof(Wall34), Wall.Width * i, _roomBounds.Height - Wall.Width * 28);
                AddWall(typeof(Wall35), Wall.Width * 7, _roomBounds.Height - Wall.Width * 28);
                for (int i = 29; i * Wall.Width <= _roomBounds.Height; i++)
                    AddWall(typeof(Wall15), Wall.Width * 7, _roomBounds.Height - Wall.Width * i);
                for (int i = 5; i < 8; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 17);
                AddWall(typeof(Wall30), Wall.Width * 8, _roomBounds.Height - Wall.Width * 17);
                AddWall(typeof(Wall30), Wall.Width * 10, _roomBounds.Height - Wall.Width * 16);
                AddWall(typeof(Wall7), Wall.Width * 12, _roomBounds.Height - Wall.Width * 15);
                for (int i = 14; i < 15; i++)
                    AddWall(typeof(Wall26), Wall.Width * 12, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall8), Wall.Width * 12, _roomBounds.Height - Wall.Width * 13);
                for (int i = 14; i < 20; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 12);
                AddWall(typeof(Wall31), Wall.Width * 20, _roomBounds.Height - Wall.Width * 12);
                AddWall(typeof(Wall32), Wall.Width * 21, _roomBounds.Height - Wall.Width * 12);
                for (int i = 13; i < 28; i++)
                    AddWall(typeof(Wall28), Wall.Width * 21, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall12), Wall.Width * 21, _roomBounds.Height - Wall.Width * 28);
                AddWall(typeof(Wall18), Wall.Width * 14, _roomBounds.Height - Wall.Width * 21);
                AddWall(typeof(Wall16), Wall.Width * 13, _roomBounds.Height - Wall.Width * 21);
                AddWall(typeof(Wall23), Wall.Width * 15, _roomBounds.Height - Wall.Width * 28);
                AddWall(typeof(Wall4), Wall.Width * 16, _roomBounds.Height - Wall.Width * 29);
                AddWall(typeof(Wall10), Wall.Width * 17, _roomBounds.Height - Wall.Width * 29);
                AddWall(typeof(Wall15), Wall.Width * 17, _roomBounds.Height - Wall.Width * 28);
                AddWall(typeof(Wall11), Wall.Width * 17, _roomBounds.Height - Wall.Width * 27);
                for (int i = 22; i < 24; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 28);
                AddWall(typeof(Wall10), Wall.Width * 24, _roomBounds.Height - Wall.Width * 28);
                for (int i = 15; i < 28; i++)
                    AddWall(typeof(Wall15), Wall.Width * 24, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall14), Wall.Width * 24, _roomBounds.Height - Wall.Width * 14);
                AddWall(typeof(Wall13), Wall.Width * 25, _roomBounds.Height - Wall.Width * 14);
                for (int i = 25; i < 36; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 14);
                AddWall(typeof(Wall32), Wall.Width * 36, _roomBounds.Height - Wall.Width * 14);
                for (int i = 15; i < 25; i++)
                    AddWall(typeof(Wall28), Wall.Width * 36, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall18), Wall.Width * 31, _roomBounds.Height - Wall.Width * 28);
                AddWall(typeof(Wall16), Wall.Width * 30, _roomBounds.Height - Wall.Width * 28);
                AddWall(typeof(Wall12), Wall.Width * 36, _roomBounds.Height - Wall.Width * 25);
                AddWall(typeof(Wall10), Wall.Width * 37, _roomBounds.Height - Wall.Width * 25);
                for (int i = 14; i < 25; i++)
                    AddWall(typeof(Wall15), Wall.Width * 37, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall11), Wall.Width * 37, _roomBounds.Height - Wall.Width * 13);
                for (int i = 4; Wall.Width * i < _roomBounds.Height; i++)
                    AddWall(typeof(Wall5), Wall.Width * 39, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall9), Wall.Width * 38, _roomBounds.Height - Wall.Width * 3);
                for (int i = 18; i < 38; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 2);
                AddWall(typeof(Wall8), Wall.Width * 16, _roomBounds.Height - Wall.Width * 3);
                for (int i = 4; i < 7; i++)
                    AddWall(typeof(Wall26), Wall.Width * 16, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall29), Wall.Width * 16, _roomBounds.Height - Wall.Width * 7);
                AddWall(typeof(Wall25), Wall.Width * 16, _roomBounds.Height - Wall.Width * 8);
                AddWall(typeof(Wall11), Wall.Width * 17, _roomBounds.Height - Wall.Width * 8);
                for (int i = 9; i < 11; i++)
                    AddWall(typeof(Wall15), Wall.Width * 17, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall36), Wall.Width * 17, _roomBounds.Height - Wall.Width * 11);
                for (int i = 18; i < 22; i++)
                    AddWall(typeof(Wall21), Wall.Width * i, _roomBounds.Height - Wall.Width * 11);
                AddWall(typeof(Wall11), Wall.Width * 22, _roomBounds.Height - Wall.Width * 11);
                AddWall(typeof(Wall15), Wall.Width * 22, _roomBounds.Height - Wall.Width * 12);
                AddWall(typeof(Wall36), Wall.Width * 22, _roomBounds.Height - Wall.Width * 13);
                for (int i = 23; i < 37; i++)
                    AddWall(typeof(Wall21), Wall.Width * i, _roomBounds.Height - Wall.Width * 13);
                AddWall(typeof(Wall23), Wall.Width * 19, _roomBounds.Height - Wall.Width * 6);
                AddWall(typeof(Wall21), Wall.Width * 21, _roomBounds.Height - Wall.Width * 5);
                AddWall(typeof(Wall21), Wall.Width * 22, _roomBounds.Height - Wall.Width * 5);
                AddWall(typeof(Wall11), Wall.Width * 23, _roomBounds.Height - Wall.Width * 5);
                for (int i = 6; i < 9; i++)
                    AddWall(typeof(Wall15), Wall.Width * 23, _roomBounds.Height - Wall.Width * i);
                AddWall(typeof(Wall5), Wall.Width * 20, _roomBounds.Height - Wall.Width * 7);
                AddWall(typeof(Wall5), Wall.Width * 20, _roomBounds.Height - Wall.Width * 8);
                AddWall(typeof(Wall4), Wall.Width * 20, _roomBounds.Height - Wall.Width * 9);
                for (int i = 21; i < 23; i++)
                    AddWall(typeof(Wall1), Wall.Width * i, _roomBounds.Height - Wall.Width * 9);
                AddWall(typeof(Wall10), Wall.Width * 23, _roomBounds.Height - Wall.Width * 9);
                AddWall(typeof(Wall23), Wall.Width * 33, _roomBounds.Height - Wall.Width * 10);
                AddWall(typeof(Wall10), Wall.Width * 35, _roomBounds.Height - Wall.Width * 11);
                AddWall(typeof(Wall15), Wall.Width * 35, _roomBounds.Height - Wall.Width * 10);
                AddWall(typeof(Wall11), Wall.Width * 35, _roomBounds.Height - Wall.Width * 9);
                AddWall(typeof(Wall4), Wall.Width * 34, _roomBounds.Height - Wall.Width * 11);
            }

            List<DisplayFeature> textures = new List<DisplayFeature>();
            {
                Action<string, float, float> AddTexture = delegate (string texture, float x, float y)
                {
                    DisplayFeature display = new DisplayFeature(
                        gameObject: this, 
                        texture: _contentManager.Load<Texture2D>(texture));
                    display.Position = new Vector2(x, y);
                    textures.Add(display);
                };
                for (int i = 0; Wall.Width * i < _roomBounds.Width; i++)
                    AddTexture("object3Texture", Wall.Width * i, _roomBounds.Height - Wall.Width * 1);
                for (int x = 0; x < 5; x++)
                    for (int y = 0; y < 4; y++)
                        AddTexture("object3Texture", Wall.Width * x, _roomBounds.Height - Wall.Width * y);
                for (int x = 0; x < 2; x++)
                    for (int y = 12; Wall.Width * y <= _roomBounds.Height; y++)
                        AddTexture("object3Texture", Wall.Width * x, _roomBounds.Height - Wall.Width * y);
                for (int i = 2; i < 6; i++)
                    AddTexture("object3Texture", Wall.Width * i, _roomBounds.Height - Wall.Width * 20);
                for (int x = 2; x < 7; x++)
                    for (int y = 28; Wall.Width * y <= _roomBounds.Height; y++)
                        AddTexture("object3Texture", Wall.Width * x, _roomBounds.Height - Wall.Width * y);
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

                AddBouncer(typeof(Bouncer), Wall.Width * 1, _roomBounds.Height - Wall.Width * 8, .8f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 15, _roomBounds.Height - Wall.Width * 15, .5f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 18, _roomBounds.Height - Wall.Width * 18, -.3f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 19, _roomBounds.Height - Wall.Width * 21, -1f, -.2f);
                AddBouncer(typeof(Bouncer), Wall.Width * 9, _roomBounds.Height - Wall.Width * 24, 0f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 14, _roomBounds.Height - Wall.Width * 24, 0f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 25, _roomBounds.Height - Wall.Width * 15, 0f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 26, _roomBounds.Height - Wall.Width * 19, 0f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 25, _roomBounds.Height - Wall.Width * 22, 0f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 27, _roomBounds.Height - Wall.Width * 26, -.75f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 23, _roomBounds.Height - Wall.Width * 30, 1f, -0.1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 36, _roomBounds.Height - Wall.Width * 26, -.5f, -1f);
                AddBouncer(typeof(Bouncer), Wall.Width * 29, _roomBounds.Height - Wall.Width * 6, 0f, -1f); 
            }

            _player = new MonoKitty(
                contentManager: contentManager);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 2,
            //    y: _roomBounds.Height - Wall.Width * 6);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 3,
            //    y: Wall.Width * 22);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 6,
            //    y: Wall.Width * 12);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 19,
            //    y: _roomBounds.Height - Wall.Width * 3);
            _player.Physics.Position = new Vector2(
                x: Wall.Width * 4,
                y: _roomBounds.Height - Wall.Width * 21);

            _camera = new CameraFeature(
                gameObject: this, 
                roomBounds: _roomBounds, 
                cameraBounds: graphicsDevice.Viewport.Bounds, 
                physics: _player.Physics, 
                threshold: new Point(x:600, y: 200));
            Features.Add(_camera);

            _background = contentManager.Load<Texture2D>("scene0");

            foreach (Wall wall in walls)
            {
                Children.Add(wall);
                _player.Physics.CollidablePhysics.Add(wall.Physics);
            }

            foreach (DisplayFeature texture in textures)
            {
                Features.Add(texture);
            }

            foreach (Bouncer bouncer in bouncers)
            {
                Children.Add(bouncer);
                _player.Physics.CollidablePhysics.Add(bouncer.Physics);
            }

            Children.Add(_player);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);      
            
            // If debug is active, the mouse can be used to move the player.
#if DEBUG
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 mouseDirection = new Vector2(
                    x: mouseState.X,
                    y: mouseState.Y) + _camera.Location.ToVector2() - _player.Physics.Center;
                
                if (mouseState.LeftButton == ButtonState.Pressed && mouseDirection.LengthSquared() > 800)
                {
                        mouseDirection.Normalize();
                        _player.ExternalAcceleration = mouseDirection * 1000f * 10f;
                }
                else
                {
                    _player.ExternalAcceleration = Vector2.Zero;
                }
            }
#endif
            
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
