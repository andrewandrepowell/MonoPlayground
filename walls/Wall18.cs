using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MonoPlayground
{
    internal class Wall18 : Wall
    {
        public Wall18(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object17Mask"),
                texture: contentManager.Load<Texture2D>("object18Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 0, y: 0));
            Physics.Vertices.Add(new Vector2(x: 31, y: 0));
            Physics.Vertices.Add(new Vector2(x: 96, y: 0));
            Physics.Vertices.Add(new Vector2(x: 120, y: 0));
            Physics.Vertices.Add(new Vector2(x: 124, y: 1));
            Physics.Vertices.Add(new Vector2(x: 126, y: 3));
            Physics.Vertices.Add(new Vector2(x: 127, y: 7));
            Physics.Vertices.Add(new Vector2(x: 127, y: 31));
            Physics.Vertices.Add(new Vector2(x: 127, y: 61));
            Physics.Vertices.Add(new Vector2(x: 127, y: 92));
            Physics.Vertices.Add(new Vector2(x: 96, y: 92));
            Physics.Vertices.Add(new Vector2(x: 31, y: 92));
            Physics.Vertices.Add(new Vector2(x: 0, y: 92));
        }
    }
}
