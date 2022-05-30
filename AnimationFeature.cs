using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace MonoPlayground
{
    internal class AnimationFeature : GameFeature
    {
        readonly private IList<Texture2D> _textures;
        readonly private Vector2 _origin;
        readonly private SpriteBatch _spriteBatch;
        private Vector2 _position;
        private int _currentIndex;
        private float _scale;
        private float _animationTimer;
        private float _animationTimerThreshold;
        private float _rotation;
        private bool _flip;
        private bool _play;
        private bool _repeat;
        private bool _visible;
        public AnimationFeature(
            GameObject gameObject,
            SpriteBatch spriteBatch,
            IList<Texture2D> textures,
            float scale = 1.0f,
            float animationTimerThreshold = 0.1f) : base(gameObject)
        {
            Debug.Assert(textures.Count > 0);
            Debug.Assert(textures.All(x => x.Bounds.Size == textures[0].Bounds.Size));
            Debug.Assert(scale >= 0);
            Debug.Assert(animationTimerThreshold > 0);

            _textures = textures;
            _spriteBatch = spriteBatch;
            _scale = scale;
            _origin = _textures[0].Bounds.Center.ToVector2();
            _animationTimerThreshold = animationTimerThreshold;
            Reset();
        }
        public Point Size { get => _textures[0].Bounds.Size; }
        public int Height { get => _textures[0].Height; }
        public int Width { get => _textures[0].Width;  }
        public Vector2 Position { get => _position; set => _position = value; }
        public float Rotation { get => _rotation; set => _rotation = value; }
        public bool Flip { get => _flip; set => _flip = value; }
        public bool Play { get => _play; set => _play = value; }
        public bool Repeat { get => _repeat; set => _repeat = value; }
        public bool Visible { get => _visible; set => _visible = value; }
        public float Scale { 
            get => _scale; 
            set
            {
                Debug.Assert(_scale > 0);
                _scale = value;
            }
        }
        public void Reset()
        {
            _currentIndex = 0;
            _animationTimer = 0;
            _flip = false;
            _play = false;
            _repeat = false;
            _visible = false;
        }
        public override void Draw(GameTime gameTime)
        {
            if (_play)
            {
                float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_animationTimer > 0)
                {
                    _animationTimer -= timeElapsed;
                }
                else
                {
                    _animationTimer = _animationTimerThreshold;
                    if (_currentIndex != (_textures.Count - 1))
                    {
                        _currentIndex++;
                    }
                    else
                    {
                        _currentIndex = 0;
                        if (!_repeat)
                            _play = false;
                    }
                }
            }

            if (_visible)
            {
                SpriteEffects spriteEffects = (_flip) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                _spriteBatch.Begin();
                _spriteBatch.Draw(
                    texture: _textures[_currentIndex],
                    position: _position,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: _rotation,
                    origin: _origin,
                    scale: _scale,
                    effects: spriteEffects,
                    layerDepth: 0f);
                _spriteBatch.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
