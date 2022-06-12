using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall30 : Wall
    {
        public Wall30(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object30Mask"),
                texture: contentManager.Load<Texture2D>("object30Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 0, y: 0));
            Physics.Vertices.Add(new Vector2(x: 128, y: 64));
            Physics.Vertices.Add(new Vector2(x: 255, y: 128));
        }
    }
}
