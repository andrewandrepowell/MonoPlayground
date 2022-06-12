using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall24 : Wall
    {
        public Wall24(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object8Mask"),
                texture: contentManager.Load<Texture2D>("object24Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 127, y: 0));
            Physics.Vertices.Add(new Vector2(x: 129, y: 31));
            Physics.Vertices.Add(new Vector2(x: 132, y: 53));
            Physics.Vertices.Add(new Vector2(x: 137, y: 73));
            Physics.Vertices.Add(new Vector2(x: 145, y: 91));
            Physics.Vertices.Add(new Vector2(x: 155, y: 104));
            Physics.Vertices.Add(new Vector2(x: 171, y: 115));
            Physics.Vertices.Add(new Vector2(x: 187, y: 121));
            Physics.Vertices.Add(new Vector2(x: 209, y: 125));
            Physics.Vertices.Add(new Vector2(x: 229, y: 127));
            Physics.Vertices.Add(new Vector2(x: 248, y: 128));
            Physics.Vertices.Add(new Vector2(x: 252, y: 129));
            Physics.Vertices.Add(new Vector2(x: 254, y: 131));
            Physics.Vertices.Add(new Vector2(x: 255, y: 135));
            Physics.Vertices.Add(new Vector2(x: 255, y: 154));
            Physics.Vertices.Add(new Vector2(x: 255, y: 180));
            Physics.Vertices.Add(new Vector2(x: 255, y: 206));
            Physics.Vertices.Add(new Vector2(x: 255, y: 255));
        }
    }
}
