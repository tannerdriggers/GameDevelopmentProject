using System;
using System.Collections.Generic;
using System.Text;

namespace GameContentLevelsExtension
{
    public class GameMapLayerContent
    {
        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool LinkWithCollision { get; set; }

        public int Visible { get; set; }

        public string TilesetName { get; set; }

        public bool PreRender { get; set; }

        public int Distance { get; set; }

        public int Tilesize { get; set; }

        public bool Foreground { get; set; }

        public int[][] Data { get; set; }
    }
}
