using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal abstract class GameFeature
    {
        private readonly GameObject _gameObject;

        public GameFeature(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public GameObject GameObject
        {
            get { return _gameObject; }
        }
    }
}
