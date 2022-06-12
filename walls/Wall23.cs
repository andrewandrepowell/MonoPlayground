using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall23 : Wall
    {
        public Wall23(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object9Mask"),
                texture: contentManager.Load<Texture2D>("object23Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 0, y: 255));            
            Physics.Vertices.Add(new Vector2(x: 0, y: 206));
            Physics.Vertices.Add(new Vector2(x: 0, y: 180));
            Physics.Vertices.Add(new Vector2(x: 0, y: 154));
            Physics.Vertices.Add(new Vector2(x: 0, y: 135));
            Physics.Vertices.Add(new Vector2(x: 1, y: 131));
            Physics.Vertices.Add(new Vector2(x: 3, y: 129));
            Physics.Vertices.Add(new Vector2(x: 7, y: 128));
            Physics.Vertices.Add(new Vector2(x: 26, y: 127));
            Physics.Vertices.Add(new Vector2(x: 46, y: 125));
            Physics.Vertices.Add(new Vector2(x: 68, y: 121));
            Physics.Vertices.Add(new Vector2(x: 84, y: 115));
            Physics.Vertices.Add(new Vector2(x: 100, y: 104));
            Physics.Vertices.Add(new Vector2(x: 110, y: 91));
            Physics.Vertices.Add(new Vector2(x: 118, y: 73));
            Physics.Vertices.Add(new Vector2(x: 123, y: 53));
            Physics.Vertices.Add(new Vector2(x: 126, y: 31));
            Physics.Vertices.Add(new Vector2(x: 128, y: 0));
        }
    }
}
