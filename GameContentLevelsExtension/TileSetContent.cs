using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace GameContentLevelsExtension
{
    public class TileMapTileSet
    {
        public string Source;

        public ExternalReference<TileSetContent> Reference;
    }

    public class TileSetContent
    {
        public string Name { get; set; }

        public int TileWidth { get; set; }

        public int TileHeight { get; set; }

        public int TileCount { get; set; }

        public int Columns { get; set; }

        public string ImageFilename { get; set; }

        public string ImageColorKey { get; set; }

        public TextureContent Texture { get; set; }

        public TileContent[] Tiles;
    }
}
