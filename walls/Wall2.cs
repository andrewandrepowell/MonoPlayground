using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall2 : Wall
    {
        public Wall2(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object2Mask"),
                texture: contentManager.Load<Texture2D>("object2Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 0, y: 0));
            Physics.Vertices.Add(new Vector2(x: 15, y: 3));
            Physics.Vertices.Add(new Vector2(x: 33, y: 17));
            Physics.Vertices.Add(new Vector2(x: 40, y: 26));
            Physics.Vertices.Add(new Vector2(x: 53, y: 49));
            Physics.Vertices.Add(new Vector2(x: 67, y: 78));
            Physics.Vertices.Add(new Vector2(x: 78, y: 97));
            Physics.Vertices.Add(new Vector2(x: 91, y: 112));
            Physics.Vertices.Add(new Vector2(x: 104, y: 121));
            Physics.Vertices.Add(new Vector2(x: 113, y: 125));
            Physics.Vertices.Add(new Vector2(x: 127, y: 127));
        }
    }
}
