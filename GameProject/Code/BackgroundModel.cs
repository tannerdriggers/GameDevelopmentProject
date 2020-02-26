using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code
{
    class BackgroundModel
    {
        public BoundingRectangle background;

        public BackgroundModel(BoundingRectangle rectangle)
        {
            background = rectangle;
        }
    }
}
