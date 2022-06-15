﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class Wall27 : Wall
    {
        public Wall27(ContentManager contentManager) :
            base(
                contentManager: contentManager,
                mask: contentManager.Load<Texture2D>("object1Mask"),
                texture: contentManager.Load<Texture2D>("object27Texture"))
        {
            Physics.Vertices.Add(new Vector2(x: 127, y: 127));
            Physics.Vertices.Add(new Vector2(x: 0, y: 127));
            Physics.Vertices.Add(new Vector2(x: 0, y: 0));
        }
    }
}