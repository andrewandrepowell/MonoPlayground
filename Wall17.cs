using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall17 : Wall
    {
        public Wall17(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object17Mask"),
                texture: contentManager.Load<Texture2D>("object17Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 127, y: 92));
            Physics.Vertices.Add(new Vector2(x: 96, y: 92));
            Physics.Vertices.Add(new Vector2(x: 31, y: 92));
            Physics.Vertices.Add(new Vector2(x: 0, y: 92));
            Physics.Vertices.Add(new Vector2(x: 0, y: 0));
            Physics.Vertices.Add(new Vector2(x: 31, y: 0));
            Physics.Vertices.Add(new Vector2(x: 96, y: 0));
            Physics.Vertices.Add(new Vector2(x: 127, y: 0));
        }
    }
}
