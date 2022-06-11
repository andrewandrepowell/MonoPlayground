using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall1 : Wall
    {
        public Wall1(ContentManager contentManager) : 
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object1Mask"),
                texture: contentManager.Load<Texture2D>("object1Texture"))
        {
            Physics.Vertices.Add(new Vector2(x:0, y:0));
            Physics.Vertices.Add(new Vector2(x: 63, y: 0));
            Physics.Vertices.Add(new Vector2(x: 127, y: 0));
        }
    }
}
