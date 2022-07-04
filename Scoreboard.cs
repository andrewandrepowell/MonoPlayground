using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class Scoreboard : GameObject
    {
        private const int _award = 10;
        private const int _awardSuper = 100;
        private const int _scoreInitial = 360;
        private const float _scoreDecreaseTimerThreshold = 1.0f;
        private const int _scoreDecreaseRate = 1;
        private const float _flashTimerThreshold = 0.5f;
        private static readonly Vector2 _textOffset = new Vector2(x: 35, y: 20); 
        private readonly SpriteFont _font;
        private readonly Texture2D _texture;
        private float _scoreDecreaseTimer;
        private float _flashTimer;
        private bool _scoreVisible;
        public int Score { get; private set; }
        public Vector2 Position { get; set; }
        public bool Frozen { get; private set; }
        public Scoreboard(ContentManager contentManager)
        {
            _font = contentManager.Load<SpriteFont>("font");
            _texture = contentManager.Load<Texture2D>("score0");
            Score = _scoreInitial;
            Position = new Vector2(x: 0, y: 0);
            Frozen = false;
            _scoreDecreaseTimer = _scoreDecreaseTimerThreshold;
            _flashTimer = _flashTimerThreshold;
            _scoreVisible = true;
        }
        public void AwardPoints()
        {
            Score += _award;
        }
        public void AwardSuperPoints()
        {
            Score += _awardSuper;
        }
        public void Freeze() => Frozen = true;
        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (Frozen)
            {
                if (_flashTimer > 0)
                    _flashTimer -= timeElapsed;
                else
                {
                    _flashTimer = _flashTimerThreshold;
                    _scoreVisible = !_scoreVisible;
                }
            }
            else
            {
                if (_scoreDecreaseTimer > 0)
                    _scoreDecreaseTimer -= timeElapsed;
                else
                {
                    _scoreDecreaseTimer = _scoreDecreaseTimerThreshold;
                    Score -= _scoreDecreaseRate;
                    if (Score < 0)
                        Score = 0;
                }
            }
            
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: _texture,
                destinationRectangle: new Rectangle(
                    location: Position.ToPoint(),
                    size: _texture.Bounds.Size),
                color: Color.White);
            if (_scoreVisible)
                spriteBatch.DrawString(
                    spriteFont: _font, 
                    text: $"Score: {Score}", 
                    position: Position + _textOffset, 
                    color: Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
