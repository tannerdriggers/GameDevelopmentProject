using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Code.Entities
{
    /// <summary>
    /// Model for Bubbles
    /// </summary>
    class BubbleModel
    {
        private Bubble bubbleFlyweight;

        public TimeSpan timer;
        public int frame;
        public Vector2 position;
        public Vector2 scale = new Vector2(1, 1);

        public BubbleModel(Game game, Vector2 position)
        {
            bubbleFlyweight = game.bubbleFlyweight;
            timer = new TimeSpan(0);
            position.Y -= bubbleFlyweight.FRAME_HEIGHT;
            this.position = position;
            frame = 0;
#if DEBUG
#else
            bubbleFlyweight.bubblesSound.Play();
#endif
        }
    }
}
