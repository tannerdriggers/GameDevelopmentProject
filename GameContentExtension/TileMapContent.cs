using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Content.Pipeline;

namespace GameContentExtension
{
    public class TileMapTileSet
    {
        public int FirstGID;

        public string Source;

        public ExternalReference<TileSetContent> Reference;
    }

    public class TileMapContent
    {
        public uint MapWidth { get; set; }

        public uint MapHeight { get; set; }

        public uint TileWidth { get; set; }

        public uint TileHeight { get; set; }

        public List<TileMapTileSet> TileSets = new List<TileMapTileSet>();

        public List<TileMapLayerContent> Layers = new List<TileMapLayerContent>();

    }
}
