using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary
{
    public class Size
    {
        public int X;

        public int Y;

        public Size(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Setting
    {
        public string Level;

        public Size Size;

        public string Target;

        public Setting(string level, Size size, string target)
        {
            Level = level;
            Size = size;
            Target = target;
        }
    }

    public class Entity
    {
        public string Type;

        public int X;

        public int Y;

        public Setting Settings;

        public Entity(string type, int x, int y, Setting settings)
        {
            Type = type;
            X = x;
            Y = y;
            Settings = settings;
        }
    }
}
