using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class SoundFeature : GameFeature
    {
        // https://freesound.org/people/bennychico11/sounds/110930/
        // https://creativecommons.org/licenses/by/4.0/
        // https://freesound.org/people/ProjectsU012/sounds/341025/
        readonly private SoundEffect _soundEffect;
        public SoundFeature(GameObject gameObject, SoundEffect soundEffect) : base(gameObject)
        {
            _soundEffect = soundEffect;
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
