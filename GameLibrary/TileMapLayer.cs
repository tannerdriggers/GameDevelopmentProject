using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary
{
    public class TileMapLayer
    {
        public uint[] Data;

        public int TileCount => Data.Length;

        public TileMapLayer(uint[] data)
        {
            Data = data;
        }
    }
}
