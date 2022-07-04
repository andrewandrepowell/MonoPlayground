using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall35 : Wall
    {
        public Wall35(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object1Mask"),
                texture: contentManager.Load<Texture2D>("object35Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 127, y: 127 - 127));
            Physics.Vertices.Add(new Vector2(x: 127, y: 127 - 7));
            Physics.Vertices.Add(new Vector2(x: 126, y: 127 - 3));
            Physics.Vertices.Add(new Vector2(x: 124, y: 127 - 1));
            Physics.Vertices.Add(new Vector2(x: 120, y: 127 - 0));
            Physics.Vertices.Add(new Vector2(x: 0, y: 127 - 0)); 
        }
    }
}
