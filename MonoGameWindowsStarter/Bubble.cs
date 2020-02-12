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

namespace GameProject
{

    class Bubble
    {
        Game1 game;
        TimeSpan timer;
        int frame;
        public Vector2 position;

        public bool alive;

        /// <summary>
        /// Width of a single sprite in the spritesheet
        /// - Changes based on the fish sprite used
        /// </summary>
        public int FRAME_WIDTH = 23;

        /// <summary>
        /// Height of a single sprite in the spritesheet
        /// - Changes based on the fish sprite used
        /// </summary>
        public int FRAME_HEIGHT = 40;

        /// <summary>
        /// How fast the sprite animations switch
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        Vector2 scale = new Vector2(1, 1);

        public Bubble(Game1 game, Vector2 position)
        {
            this.game = game;
            alive = true;
            timer = new TimeSpan(0);
            position.Y -= FRAME_HEIGHT;
            this.position = position;
            frame = 0;
            game.bubblesSound.Play();
        }

        public void Update(GameTime gameTime)
        {
            if (frame < 5)
            {
                timer += gameTime.ElapsedGameTime;

                while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
                {
                    frame++;
                    timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                }
            }
            else
            {
                alive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                var source = new Rectangle(
                    frame * FRAME_WIDTH,
                    0,
                    FRAME_WIDTH,
                    FRAME_HEIGHT
                );

                spriteBatch.Draw(
                    texture: game.bubblesTexture, 
                    position: position, 
                    sourceRectangle: source, 
                    color: Color.White
                );
            }
        }
    }
}
