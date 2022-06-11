using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall7 : Wall
    {
        public Wall7(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object7Mask"),
                texture: contentManager.Load<Texture2D>("object7Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 0, y: 0));
            Physics.Vertices.Add(new Vector2(x: 32, y: 4));
            Physics.Vertices.Add(new Vector2(x: 56, y: 13));
            Physics.Vertices.Add(new Vector2(x: 90, y: 38));
            Physics.Vertices.Add(new Vector2(x: 107, y: 59));
            Physics.Vertices.Add(new Vector2(x: 119, y: 83));
            Physics.Vertices.Add(new Vector2(x: 127, y: 127)); 
        }        
    }
}
