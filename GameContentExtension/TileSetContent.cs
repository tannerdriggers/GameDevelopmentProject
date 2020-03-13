using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace GameContentExtension
{
    /// <summary>
    /// A class representing the content associated with a Tiled tileset.
    /// It is used as an intermediary stage in the content pipeline
    /// </summary>
    public class TileSetContent
    {
        public string Name { get; set; }

        public int TileWidth { get; set; }

        public int TileHeight { get; set; }

        public int Spacing { get; set; }

        public int Margin { get; set; }

        public int TileCount { get; set; }

        public int Columns { get; set; }

        public string ImageFilename { get; set; }

        public string ImageColorKey { get; set; }

        public TextureContent Texture { get; set; }

        public TileContent[] Tiles;
    }
}
