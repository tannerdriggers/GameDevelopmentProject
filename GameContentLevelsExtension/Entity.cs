using System;
using System.Collections.Generic;
using System.Text;

namespace GameContentLevelsExtension
{
    public class Size
    {
        public int X { get; set; }

        public int Y { get; set; }
    }

    public class Setting
    {
        public string Level { get; set; }

        public Size Size { get; set; }

        public string Target { get; set; }
    }

    public class Entity
    {
        public string Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Setting Settings { get; set; }
    }
}
