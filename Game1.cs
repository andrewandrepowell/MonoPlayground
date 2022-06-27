using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#if DEBUG
using System.Runtime.InteropServices;
#endif

namespace MonoPlayground
{
    public class Game1 : Game
    {
        private const int _gameWidth = 1280; // 720p
        private const int _gameHeight = 720; // 720p
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private LevelRoom _roomLevel;
        private EndRoom _roomEnd;
        private Song _songGame;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
#if DEBUG
            AllocConsole();
#endif
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.HardwareModeSwitch = true;
            _graphics.PreferredBackBufferWidth = _gameWidth;
            _graphics.PreferredBackBufferHeight = _gameHeight;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _songGame = Content.Load<Song>("gameMusic");
            MediaPlayer.Play(_songGame);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.01f;

            /*          
            _roomLevel = new LevelRoom(
                contentManager: Content,
                graphicsDevice: GraphicsDevice);
            */
            
            _roomEnd = new EndRoom(game: this, score: 2);
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if (_roomLevel != null)
                if (_roomLevel.GameOver)
                {
                    Content.Unload();
                    _roomEnd = new EndRoom(game: this, score: _roomLevel.Score.Score);
                    _roomLevel = null;
                }
                else
                    _roomLevel.Update(gameTime);
            
            if (_roomEnd != null)
                _roomEnd.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            if (_roomLevel != null)
                _roomLevel.Draw(gameTime, _spriteBatch);

            if (_roomEnd != null)
                _roomEnd.Draw(gameTime, _spriteBatch);
            base.Draw(gameTime);
        }
#if DEBUG
        // https://gamedev.stackexchange.com/questions/45107/input-output-console-window-in-xna#:~:text=Right%20click%20your%20game%20in%20the%20solution%20explorer,tab.%20Change%20the%20Output%20Type%20to%20Console%20Application.
        [DllImport("kernel32")]
        static extern bool AllocConsole();
#endif
    }
}
