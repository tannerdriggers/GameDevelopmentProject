using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameContentLevelsExtension
{
    public class TileContent
    {
        /// <summary>
        /// The Tile's source rectangle
        /// </summary>
        public Rectangle Source { get; set; }

        /// <summary>
        /// If the tile is solid
        /// </summary>
        public bool Solid { get; set; }

        /// <summary>
        /// Creates a new TileContent with the specified source rectangle
        /// </summary>
        /// <param name="source">The source rectangle of the tile in its spritesheet</param>
        public TileContent(Rectangle source)
        {
            Source = source;
        }
    }
}
