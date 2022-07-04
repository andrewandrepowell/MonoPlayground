using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall9 : Wall
    {
        public Wall9(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object9Mask"),
                texture: contentManager.Load<Texture2D>("object9Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 255 - 255, y: 128));
            Physics.Vertices.Add(new Vector2(x: 255 - 229, y: 127));
            Physics.Vertices.Add(new Vector2(x: 255 - 209, y: 125));
            Physics.Vertices.Add(new Vector2(x: 255 - 187, y: 121));
            Physics.Vertices.Add(new Vector2(x: 255 - 171, y: 115));
            Physics.Vertices.Add(new Vector2(x: 255 - 155, y: 104));
            Physics.Vertices.Add(new Vector2(x: 255 - 145, y: 91));
            Physics.Vertices.Add(new Vector2(x: 255 - 137, y: 73));
            Physics.Vertices.Add(new Vector2(x: 255 - 132, y: 53));
            Physics.Vertices.Add(new Vector2(x: 255 - 129, y: 31));
            Physics.Vertices.Add(new Vector2(x: 255 - 127, y: 0));
        }
    }
}
