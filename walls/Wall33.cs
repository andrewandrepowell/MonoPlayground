using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall33 : Wall
    {
        public Wall33(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object33Mask"),
                texture: contentManager.Load<Texture2D>("object33Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 255, y: 255 - 128));
            Physics.Vertices.Add(new Vector2(x: 229, y: 255 - 127));
            Physics.Vertices.Add(new Vector2(x: 209, y: 255 - 125));
            Physics.Vertices.Add(new Vector2(x: 187, y: 255 - 121));
            Physics.Vertices.Add(new Vector2(x: 171, y: 255 - 115));
            Physics.Vertices.Add(new Vector2(x: 155, y: 255 - 104));
            Physics.Vertices.Add(new Vector2(x: 145, y: 255 - 91));
            Physics.Vertices.Add(new Vector2(x: 137, y: 255 - 73));
            Physics.Vertices.Add(new Vector2(x: 132, y: 255 - 53));
            Physics.Vertices.Add(new Vector2(x: 129, y: 255 - 31));
            Physics.Vertices.Add(new Vector2(x: 127, y: 255 - 0));  
        }
    }
}
