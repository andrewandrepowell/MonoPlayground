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
        private TitleRoom _roomTitle;
        private AboutRoom _roomAbout;
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
            // Configuring the settings of the game.
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.HardwareModeSwitch = true;
            _graphics.PreferredBackBufferWidth = _gameWidth;
            _graphics.PreferredBackBufferHeight = _gameHeight;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Load the content of the game.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Play the game's music.
            _songGame = Content.Load<Song>("gameMusic");
            MediaPlayer.Play(_songGame);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.01f;

            // Load the title screen.
            _roomTitle = new TitleRoom(game: this);
        }

        protected override void Update(GameTime gameTime)
        {
            // Use Monogame's default way to implement game close.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update title room.
            // If the game is started, create the level room.
            // If the about page is activated, create the about room.
            if (_roomTitle != null)
                if (_roomTitle.GameStarted)
                {
                    _roomTitle = null;
                    _roomLevel = new LevelRoom(this);
                }
                else if (_roomTitle.About)
                {
                    _roomTitle = null;
                    _roomAbout = new AboutRoom(this);
                }
                else
                    _roomTitle.Update(gameTime);

            // Update about room. If the title is activated, go back to the title screen.
            if (_roomAbout != null)
                if (_roomAbout.Title)
                {
                    _roomAbout = null;
                    _roomTitle = new TitleRoom(game: this);
                }
                else
                    _roomAbout.Update(gameTime);

            // Update level room. If the game is over, create the end room.
            if (_roomLevel != null)
                if (_roomLevel.GameOver)
                {
                    _roomEnd = new EndRoom(game: this, score: _roomLevel.Score.Score);
                    _roomLevel = null;
                }
                else
                    _roomLevel.Update(gameTime);
            
            // Update end room.
            if (_roomEnd != null)
                _roomEnd.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            // Draw the rooms if they exist.
            
            GraphicsDevice.Clear(Color.White);

            if (_roomTitle != null)
                _roomTitle.Draw(gameTime, _spriteBatch);

            if (_roomAbout != null)
                _roomAbout.Draw(gameTime, _spriteBatch);

            if (_roomLevel != null)
                _roomLevel.Draw(gameTime, _spriteBatch);

            if (_roomEnd != null)
                _roomEnd.Draw(gameTime, _spriteBatch);
            base.Draw(gameTime);
        }
#if DEBUG
        // https://gamedev.stackexchange.com/questions/45107/input-output-console-window-in-xna#:~:text=Right%20click%20your%20game%20in%20the%20solution%20explorer,tab.%20Change%20the%20Output%20Type%20to%20Console%20Application.
        // This opens a console window in the game.
        [DllImport("kernel32")]
        static extern bool AllocConsole();
#endif
    }
}
