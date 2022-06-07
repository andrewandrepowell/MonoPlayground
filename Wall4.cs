using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall4 : Wall
    {
        public Wall4(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object7Mask"),
                texture: contentManager.Load<Texture2D>("object4Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 0, y: 127));
            Physics.Vertices.Add(new Vector2(x: 8, y: 83));
            Physics.Vertices.Add(new Vector2(x: 20, y: 59));
            Physics.Vertices.Add(new Vector2(x: 37, y: 38));
            Physics.Vertices.Add(new Vector2(x: 71, y: 13));
            Physics.Vertices.Add(new Vector2(x: 95, y: 4));
            Physics.Vertices.Add(new Vector2(x: 127, y: 0));            
        }
    }
}
