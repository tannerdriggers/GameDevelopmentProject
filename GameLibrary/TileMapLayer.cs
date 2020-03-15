using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary
{
    public class TileMapLayer
    {
        public string Name { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool LinkWithCollision { get; set; }

        public int Visible { get; set; }

        public string TilesetName { get; set; }

        public bool Repeat { get; set; }

        public bool PreRender { get; set; }

        public int Distance { get; set; }

        public int Tilesize { get; set; }

        public bool Foreground { get; set; }

        public int[][] Data { get; set; }

        public int TileCount => Data.Length;

        public TileMapLayer(
            string name, 
            int width, 
            int height, 
            bool linkWithCollision, 
            int visible, 
            string tilesetName, 
            bool repeat,
            bool preRender,
            int distance,
            int tilesize,
            bool foreground,
            int[][] data)
        {
            Name = name;
            Width = width;
            Height = height;
            LinkWithCollision = linkWithCollision;
            Visible = visible;
            TilesetName = tilesetName;
            Repeat = repeat;
            PreRender = preRender;
            Distance = distance;
            Tilesize = tilesize;
            Foreground = foreground;
            Data = data;
        }
    }
}
