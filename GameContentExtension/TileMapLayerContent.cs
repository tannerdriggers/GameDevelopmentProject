using System;
using System.Collections.Generic;
using System.Text;

namespace GameContentExtension
{
    public class TileMapLayerContent
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public string DataString { get; set; }

        public uint[] Data { get; set; }
    }
}
