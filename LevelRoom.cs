using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoPlayground
{
    internal class LevelRoom : GameObject
    {
        private static readonly Rectangle _roomBounds = new Rectangle(
            x: 0, y: 0, 
            width: Wall.Width * 40, 
            height: Wall.Width * 30);
        private readonly MonoKitty _player;
        private readonly Flag _flag;
        private readonly CameraFeature _camera;
        private readonly DisplayFeature _background;
        private readonly Scoreboard _scoreboard;
        private readonly Fader _fader;
        private readonly SoundEffectInstance _soundGameOver;
        public bool GameOver { get; private set; }
        public Scoreboard Score { get => _scoreboard; }
        public LevelRoom(Game game)
        {
            GameOver = false;

            List<Wall> walls = new List<Wall>();
            {
                void AddWall<T>(float x, float y) where T : Wall
                {
                    T wall = (T)Activator.CreateInstance(typeof(T), game.Content);
                    wall.Physics.Position = new Vector2(x: Wall.Width * x, y: _roomBounds.Height - Wall.Width * y);
                    walls.Add(wall);
                };
                
                for (int i = 5; i < 12; i++)
                    AddWall<Wall15>(0, i);
                AddWall<Wall14>(0, 4);
                AddWall<Wall13>(1, 4);
                for (int i = 2; i < 5; i++)
                    AddWall<Wall1>(i, 4);
                AddWall<Wall7>(5, 4);
                AddWall<Wall8>(5, 3);
                for (int i = 7; i < 14; i++)
                    AddWall<Wall1>(i, 2);
                AddWall<Wall9>(14, 3);
                for (int i = 4; i < 8; i++)
                    AddWall<Wall5>(15, i);
                AddWall<Wall19>(15, 8);
                AddWall<Wall18>(11, 6);
                AddWall<Wall17>(10, 6);
                AddWall<Wall16>(9, 6);
                AddWall<Wall20>(15, 9);
                for (int i = 8; i < 15; i++)
                    AddWall<Wall21>(i, 9);
                AddWall<Wall18>(6, 6);
                AddWall<Wall16>(5, 6);
                AddWall<Wall23>(6, 10);
                AddWall<Wall18>(3, 7);
                AddWall<Wall16>(2, 7);
                for (int i = 11; i < 14; i++)
                    AddWall<Wall5>(7, i);
                AddWall<Wall19>(7, 14);
                AddWall<Wall20>(7, 15);
                for (int i = 5; i < 7; i++)
                    AddWall<Wall21>(i, 15);
                AddWall<Wall27>(4, 15);
                for (int i = 16; i < 17; i++)
                    AddWall<Wall28>(4, i);
                AddWall<Wall12>(4, 17);
                AddWall<Wall24>(2, 13);
                AddWall<Wall21>(1, 12);
                AddWall<Wall36>(0, 12);
                for (int i = 14; i < 18; i++)
                    AddWall<Wall26>(2, i);
                AddWall<Wall29>(2, 18);
                AddWall<Wall25>(2, 19);
                for (int i = 3; i < 8; i++)
                    AddWall<Wall21>(i, 19);
                AddWall<Wall11>(8, 19);
                AddWall<Wall10>(8, 20);
                AddWall<Wall30>(6, 21);
                for (int i = 4; i < 6; i++)
                    AddWall<Wall1>(i, 21);
                AddWall<Wall8>(2, 22);
                for (int i = 23; i < 27; i++)
                    AddWall<Wall26>(2, i);
                AddWall<Wall33>(2, 28);
                for (int i = 4; i < 7; i++)
                    AddWall<Wall34>(i, 28);
                AddWall<Wall35>(7, 28);
                for (int i = 29; i * Wall.Width <= _roomBounds.Height; i++)
                    AddWall<Wall15>(7, i);
                for (int i = 5; i < 8; i++)
                    AddWall<Wall1>(i, 17);
                AddWall<Wall30>(8, 17);
                AddWall<Wall30>(10, 16);
                AddWall<Wall7>(12, 15);
                for (int i = 14; i < 15; i++)
                    AddWall<Wall26>(12, i);
                AddWall<Wall8>(12, 13);
                for (int i = 14; i < 20; i++)
                    AddWall<Wall1>(i, 12);
                AddWall<Wall31>(20, 12);
                AddWall<Wall32>(21, 12);
                for (int i = 13; i < 28; i++)
                    AddWall<Wall28>(21, i);
                AddWall<Wall12>(21, 28);
                AddWall<Wall18>(14, 21);
                AddWall<Wall16>(13, 21);
                AddWall<Wall23>(15, 28);
                AddWall<Wall4>(16, 29);
                AddWall<Wall10>(17, 29);
                AddWall<Wall15>(17, 28);
                AddWall<Wall11>(17, 27);
                for (int i = 22; i < 24; i++)
                    AddWall<Wall1>(i, 28);
                AddWall<Wall10>(24, 28);
                for (int i = 15; i < 28; i++)
                    AddWall<Wall15>(24, i);
                AddWall<Wall14>(24, 14);
                AddWall<Wall13>(25, 14);
                for (int i = 25; i < 36; i++)
                    AddWall<Wall1>(i, 14);
                AddWall<Wall32>(36, 14);
                for (int i = 15; i < 25; i++)
                    AddWall<Wall28>(36, i);
                AddWall<Wall18>(31, 28);
                AddWall<Wall16>(30, 28);
                AddWall<Wall12>(36, 25);
                AddWall<Wall10>(37, 25);
                for (int i = 14; i < 25; i++)
                    AddWall<Wall15>(37, i);
                AddWall<Wall11>(37, 13);
                for (int i = 4; Wall.Width * i <= _roomBounds.Height; i++)
                    AddWall<Wall5>(39, i);
                AddWall<Wall9>(38, 3);
                for (int i = 18; i < 38; i++)
                    AddWall<Wall1>(i, 2);
                AddWall<Wall8>(16, 3);
                for (int i = 4; i < 7; i++)
                    AddWall<Wall26>(16, i);
                AddWall<Wall29>(16, 7);
                AddWall<Wall25>(16, 8);
                AddWall<Wall11>(17, 8);
                for (int i = 9; i < 11; i++)
                    AddWall<Wall15>(17, i);
                AddWall<Wall36>(17, 11);
                for (int i = 18; i < 22; i++)
                    AddWall<Wall21>(i, 11);
                AddWall<Wall11>(22, 11);
                AddWall<Wall15>(22, 12);
                AddWall<Wall36>(22, 13);
                for (int i = 23; i < 37; i++)
                    AddWall<Wall21>(i, 13);
                AddWall<Wall23>(19, 6);
                AddWall<Wall21>(21, 5);
                AddWall<Wall21>(22, 5);
                AddWall<Wall11>(23, 5);
                for (int i = 6; i < 9; i++)
                    AddWall<Wall15>(23, i);
                AddWall<Wall5>(20, 7);
                AddWall<Wall5>(20, 8);
                AddWall<Wall4>(20, 9);
                for (int i = 21; i < 23; i++)
                    AddWall<Wall1>(i, 9);
                AddWall<Wall10>(23, 9);
                AddWall<Wall23>(33, 10);
                AddWall<Wall10>(35, 11);
                AddWall<Wall15>(35, 10);
                AddWall<Wall11>(35, 9);
                AddWall<Wall4>(34, 11);
            }

            List<DisplayFeature> textures = new List<DisplayFeature>();
            {
                void AddTexture(string texture, float x, float y)
                {
                    DisplayFeature display = new DisplayFeature(
                        gameObject: this, 
                        texture: game.Content.Load<Texture2D>(texture));
                    display.Position = new Vector2(Wall.Width * x, _roomBounds.Height - Wall.Width * y);
                    textures.Add(display);
                };
                
                for (int i = 0; Wall.Width * i < _roomBounds.Width; i++)
                    AddTexture("object3Texture",i, 1);
                for (int x = 0; x < 5; x++)
                    for (int y = 0; y < 4; y++)
                        AddTexture("object3Texture", x, y);
                for (int x = 0; x < 2; x++)
                    for (int y = 12; Wall.Width * y <= _roomBounds.Height; y++)
                        AddTexture("object3Texture", x, y);
                for (int i = 2; i < 6; i++)
                    AddTexture("object3Texture", i, 20);
                for (int x = 2; x < 7; x++)
                    for (int y = 28; Wall.Width * y <= _roomBounds.Height; y++)
                        AddTexture("object3Texture", x, y);
                for (int i = 5; i < 8; i++)
                    AddTexture("object3Texture", i, 16);
                AddTexture("object3Texture", 8, 15);
                AddTexture("object3Texture", 9, 15);
                for (int i = 8; i < 12; i++)
                    AddTexture("object3Texture", Wall.Width * i, _roomBounds.Height - Wall.Width * 14);
                for (int y = 10; y < 14; y++)
                    for (int x = 8; x < 12; x++)
                        AddTexture("object3Texture", x, y);
                AddTexture("object3Texture", 16, 9);
                for (int y = 10; y < 12; y++)
                    for (int x = 12; x < 17; x++)
                        AddTexture("object3Texture", x, y);
                for (int y = 14; y < 28; y++)
                    for (int x = 22; x < 24; x++)
                        AddTexture("object3Texture", x, y);
                for (int y = 6; y < 9; y++)
                    for (int x = 21; x < 23; x++)
                        AddTexture("object3Texture", x, y);
            }

            List<Bouncer> bouncers = new List<Bouncer>();
            {
                void AddBouncer(float posx, float posy, float dirx, float diry)
                {
                    Bouncer bouncer = new Bouncer(game.Content);
                    bouncer.Physics.Position = new Vector2(x: posx, y: posy);
                    bouncer.Direction = new Vector2(x: dirx, y: diry);
                    bouncers.Add(bouncer);
                };

                AddBouncer(Wall.Width * 1, _roomBounds.Height - Wall.Width * 8, .8f, -1f);
                AddBouncer(Wall.Width * 15, _roomBounds.Height - Wall.Width * 15, .5f, -1f);
                AddBouncer(Wall.Width * 18, _roomBounds.Height - Wall.Width * 18, -.3f, -1f);
                AddBouncer(Wall.Width * 19, _roomBounds.Height - Wall.Width * 21, -1f, -.2f);
                AddBouncer(Wall.Width * 9, _roomBounds.Height - Wall.Width * 24, 0f, -1f);
                AddBouncer(Wall.Width * 14, _roomBounds.Height - Wall.Width * 24, 0f, -1f);
                AddBouncer(Wall.Width * 25, _roomBounds.Height - Wall.Width * 15, 0f, -1f);
                AddBouncer(Wall.Width * 26, _roomBounds.Height - Wall.Width * 19, 0f, -1f);
                AddBouncer(Wall.Width * 25, _roomBounds.Height - Wall.Width * 22, 0f, -1f);
                AddBouncer(Wall.Width * 27, _roomBounds.Height - Wall.Width * 26, -.75f, -1f);
                AddBouncer(Wall.Width * 23, _roomBounds.Height - Wall.Width * 30, 1f, -0.1f);
                AddBouncer(Wall.Width * 36, _roomBounds.Height - Wall.Width * 26, -.5f, -1f);
                AddBouncer(Wall.Width * 29, _roomBounds.Height - Wall.Width * 6, 0f, -1f); 
            }

            List<Cookie> cookies = new List<Cookie>();
            {
                void AddCookie(float x, float y)
                {
                    Cookie cookie = new Cookie(game.Content);
                    cookie.Physics.Position = new Vector2(x, y);
                    cookies.Add(cookie);
                };
                AddCookie(Wall.Width * 3, _roomBounds.Height - Wall.Width * 5);
                AddCookie(Wall.Width * 10, _roomBounds.Height - Wall.Width * 3);
                AddCookie(Wall.Width * 14, _roomBounds.Height - Wall.Width * 8);
                AddCookie(Wall.Width * 3.5f, _roomBounds.Height - Wall.Width * 9.5f);
                AddCookie(Wall.Width * 6, _roomBounds.Height - Wall.Width * 14);
                AddCookie(Wall.Width * 3, _roomBounds.Height - Wall.Width * 18);
                AddCookie(Wall.Width * 13.5f, _roomBounds.Height - Wall.Width * 16);
                AddCookie(Wall.Width * 20f, _roomBounds.Height - Wall.Width * 13);
                AddCookie(Wall.Width * 19.5f, _roomBounds.Height - Wall.Width * 13);
                AddCookie(Wall.Width * 19f, _roomBounds.Height - Wall.Width * 13);
                AddCookie(Wall.Width * 18.5f, _roomBounds.Height - Wall.Width * 13);
                AddCookie(Wall.Width * 18f, _roomBounds.Height - Wall.Width * 13);
                AddCookie(Wall.Width * 16f, _roomBounds.Height - Wall.Width * 17);
                AddCookie(Wall.Width * 17f, _roomBounds.Height - Wall.Width * 21);
                AddCookie(Wall.Width * 13.5f, _roomBounds.Height - Wall.Width * 22);
                AddCookie(Wall.Width * 3f, _roomBounds.Height - Wall.Width * 23);
                AddCookie(Wall.Width * 3f, _roomBounds.Height - Wall.Width * 24);
                AddCookie(Wall.Width * 3f, _roomBounds.Height - Wall.Width * 25);
                AddCookie(Wall.Width * 3f, _roomBounds.Height - Wall.Width * 26);
                AddCookie(Wall.Width * 5f, _roomBounds.Height - Wall.Width * 27);
                AddCookie(Wall.Width * 6f, _roomBounds.Height - Wall.Width * 27);
                AddCookie(Wall.Width * 7f, _roomBounds.Height - Wall.Width * 27);
                AddCookie(Wall.Width * 11f, _roomBounds.Height - Wall.Width * 27);
                AddCookie(Wall.Width * 15, _roomBounds.Height - Wall.Width * 28);
                AddCookie(Wall.Width * 15.3f, _roomBounds.Height - Wall.Width * 29.5f);
                AddCookie(Wall.Width * 16.5f, _roomBounds.Height - Wall.Width * 30f);
                AddCookie(Wall.Width * 27.5f, _roomBounds.Height - Wall.Width * 30f);
                AddCookie(Wall.Width * 31.5f, _roomBounds.Height - Wall.Width * 29f);
                AddCookie(Wall.Width * 35f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 34.5f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 34f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 33.5f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 33f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 32.5f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 32f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 31.5f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 31f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 25f, _roomBounds.Height - Wall.Width * 17f);
                AddCookie(Wall.Width * 26f, _roomBounds.Height - Wall.Width * 23f);
                AddCookie(Wall.Width * 26f, _roomBounds.Height - Wall.Width * 27f);
                AddCookie(Wall.Width * 38f, _roomBounds.Height - Wall.Width * 19f);
                AddCookie(Wall.Width * 38f, _roomBounds.Height - Wall.Width * 18f);
                AddCookie(Wall.Width * 38f, _roomBounds.Height - Wall.Width * 17f);
                AddCookie(Wall.Width * 38f, _roomBounds.Height - Wall.Width * 16f);
                AddCookie(Wall.Width * 38f, _roomBounds.Height - Wall.Width * 15f);
                AddCookie(Wall.Width * 30f, _roomBounds.Height - Wall.Width * 3f);
                AddCookie(Wall.Width * 29f, _roomBounds.Height - Wall.Width * 3f);
                AddCookie(Wall.Width * 28f, _roomBounds.Height - Wall.Width * 3f);
                AddCookie(Wall.Width * 17f, _roomBounds.Height - Wall.Width * 7f);
                AddCookie(Wall.Width * 23f, _roomBounds.Height - Wall.Width * 10f);
                AddCookie(Wall.Width * 29f, _roomBounds.Height - Wall.Width * 7f);
                AddCookie(Wall.Width * 33f, _roomBounds.Height - Wall.Width * 10f);
            }

            List<Bush> bushes = new List<Bush>();
            {
                void AddBush(float x, float y) => bushes.Add(new Bush(game) { Position = new Vector2(x, y) });
                AddBush(Wall.Width * 4, _roomBounds.Height - Wall.Width * 5);
                AddBush(Wall.Width * 1, _roomBounds.Height - Wall.Width * 5);
                AddBush(Wall.Width * 8, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 12, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 9, _roomBounds.Height - Wall.Width * 7);
                AddBush(Wall.Width * 5.5f, _roomBounds.Height - Wall.Width * 7);
                AddBush(Wall.Width * 4, _roomBounds.Height - Wall.Width * 18);
                AddBush(Wall.Width * 5, _roomBounds.Height - Wall.Width * 18);
                AddBush(Wall.Width * 6, _roomBounds.Height - Wall.Width * 18);
                AddBush(Wall.Width * 7, _roomBounds.Height - Wall.Width * 18);
                AddBush(Wall.Width * 14, _roomBounds.Height - Wall.Width * 13);
                AddBush(Wall.Width * 15, _roomBounds.Height - Wall.Width * 13);
                AddBush(Wall.Width * 17, _roomBounds.Height - Wall.Width * 13);
                AddBush(Wall.Width * 20, _roomBounds.Height - Wall.Width * 13);
                AddBush(Wall.Width * 4, _roomBounds.Height - Wall.Width * 22);
                AddBush(Wall.Width * 17, _roomBounds.Height - Wall.Width * 30);
                AddBush(Wall.Width * 21, _roomBounds.Height - Wall.Width * 29);
                AddBush(Wall.Width * 24, _roomBounds.Height - Wall.Width * 29);
                AddBush(Wall.Width * 27, _roomBounds.Height - Wall.Width * 15);
                AddBush(Wall.Width * 28, _roomBounds.Height - Wall.Width * 15);
                AddBush(Wall.Width * 29, _roomBounds.Height - Wall.Width * 15);
                AddBush(Wall.Width * 30, _roomBounds.Height - Wall.Width * 15);
                AddBush(Wall.Width * 34, _roomBounds.Height - Wall.Width * 15);
                AddBush(Wall.Width * 30.5f, _roomBounds.Height - Wall.Width * 29);
                AddBush(Wall.Width * 37, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 36, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 34, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 32, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 31, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 30, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 29, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 27, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 25, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 24, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 22, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 21, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 20, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 18, _roomBounds.Height - Wall.Width * 3);
                AddBush(Wall.Width * 21, _roomBounds.Height - Wall.Width * 10);
                AddBush(Wall.Width * 23, _roomBounds.Height - Wall.Width * 10);
            }

            List<Tree> trees = new List<Tree>();
            {
                void AddTree(float x, float y) => 
                    trees.Add(new Tree(game) { Position = new Vector2(x: Wall.Width * x, y: _roomBounds.Height - Wall.Width * y)  });
                AddTree(2, 7);
                AddTree(9, 5);
                AddTree(14.5f, 15);
                AddTree(18, 15);
                AddTree(21.5f, 31);
                AddTree(31, 17);
                AddTree(27.5f, 17);
                AddTree(33.5f, 5);
                AddTree(28.5f, 5);
                AddTree(24.5f, 5);
            }

            _flag = new Flag(contentManager: game.Content);
            _flag.Physics.Position = new Vector2(x: Wall.Width * 35, y: _roomBounds.Height - Wall.Width * 12);

            _scoreboard = new Scoreboard(game.Content);

            _player = new MonoKitty(
                contentManager: game.Content);
            _player.Scoreboard = _scoreboard;
            _player.Physics.Position = new Vector2(
                x: Wall.Width * 2,
                y: _roomBounds.Height - Wall.Width * 6);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 3,
            //    y: Wall.Width * 22);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 6,
            //    y: Wall.Width * 12);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 19,
            //    y: _roomBounds.Height - Wall.Width * 3);
            //_player.Physics.Position = new Vector2(
            //    x: Wall.Width * 4,
            //    y: _roomBounds.Height - Wall.Width * 21);

            _camera = new CameraFeature(
                gameObject: this, 
                roomBounds: _roomBounds, 
                cameraBounds: game.GraphicsDevice.Viewport.Bounds, 
                physics: _player.Physics, 
                threshold: new Point(x:600, y: 200));
            Features.Add(_camera);

            _background = new DisplayFeature(gameObject: this, texture: game.Content.Load<Texture2D>("scene0"));
            Features.Add(_background);

            _fader = new Fader(cameraBounds: _camera.CameraBounds, color: Color.White);
            _fader.Alpha = 1.0f;
            _fader.FadeIn();

            _soundGameOver = game.Content.Load<SoundEffect>("endSound").CreateInstance();
            _soundGameOver.Volume = 0.01f;

            foreach (Wall wall in walls)
            {
                Children.Add(wall);
                _player.Physics.CollidablePhysics.Add(wall.Physics);
            }

            foreach (DisplayFeature texture in textures)
            {
                Features.Add(texture);
            }

            foreach (Tree tree in trees)
                Children.Add(tree);

            foreach (Bush bush in bushes)
                Children.Add(bush);

            foreach (Bouncer bouncer in bouncers)
            {
                Children.Add(bouncer);
                _player.Physics.CollidablePhysics.Add(bouncer.Physics);
            }

            foreach (Cookie cookie in cookies)
            {
                Children.Add(cookie);
                _player.Physics.CollidablePhysics.Add(cookie.Physics);
            }
            Children.Add(_flag);
            _player.Physics.CollidablePhysics.Add(_flag.Physics);

            Children.Add(_player);
            Children.Add(_scoreboard);
            Children.Add(_fader);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);      
            
            // If debug is active, the mouse can be used to move the player.
#if DEBUG
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 mousePosition = mouseState.Position.ToVector2() + _camera.Location.ToVector2();
                Vector2 mouseDirection = mousePosition - _player.Physics.Center;
                Console.WriteLine($"Mouse Position X: {mousePosition.X / Wall.Width}, Y: {(_roomBounds.Height - mousePosition.Y) / Wall.Width}");
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
            // Check the condition for ending the level.
            if (_flag.Touched)
            {
                if (_fader.Alpha == 0.0f)
                {
                    _fader.FadeOut();
                    _soundGameOver.Play();
                }
                else if (_fader.Alpha == 1.0f)
                    GameOver = true;
            }

            // Clean up destroyed game objects.            
            Children.Where(x => x.Destroyed).ToList().ForEach(x => Children.Remove(x));

            // Update positions of background and scoreboard based on where the camera is.
            _background.Position = _camera.Location.ToVector2();
            _scoreboard.Position = _camera.Location.ToVector2();
            _fader.Position = _camera.Location;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Transform matrix from the camera is applied to implement camera movement.
            spriteBatch.Begin(transformMatrix: _camera.Transform);

            // Draw the rest of the game objects.
            base.Draw(gameTime, spriteBatch);
            
            
            spriteBatch.End();
        }
    }
}
