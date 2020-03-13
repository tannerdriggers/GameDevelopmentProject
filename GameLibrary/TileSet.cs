using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary
{
    public class TileSet
    {
        Tile[] tiles;

        /// <summary>
        /// Constructs a new instance of Tileset
        /// </summary>
        /// <param name="tiles">The tiles in this set</param>
        public TileSet(Tile[] tiles)
        {
            this.tiles = tiles;
        }

        /// <summary>
        /// An indexer for accessing individual tiles in the set
        /// </summary>
        /// <param name="index">The index of the tile</param>
        /// <returns>The sprite</returns>
        public Tile this[int index] {
            get => tiles[index];
        }

        /// <summary>
        /// The number of tiles in the set
        /// </summary>
        public int Count => tiles.Length;
    }
}
