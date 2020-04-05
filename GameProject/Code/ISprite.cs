﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code
{
    /// <summary>
    /// Interface representing a sprite to be drawn with a SpriteBatch
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Draws the ISprite.  This method should be invoked between calls to 
        /// SpriteBatch.Begin() and SpriteBatch.End() with the supplied SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        void Draw(SpriteBatch spriteBatch);
    }
}
