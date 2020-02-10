using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GameProject
{
    class Enemy
    {
        Game1 game;
        Texture2D fish;
        Texture2D fish_big;
        Texture2D fish_dart;

        enum playerState {
            idle = 0,
            swimming = 1,
            hurt = 4
        }

        public Enemy(Game1 game)
        {
            this.game = game;
        }

        public void LoadContent(ContentManager Content)
        {
            fish = Content.Load<Texture2D>("fish");
            fish_big = Content.Load<Texture2D>("fish_big");
            fish_dart = Content.Load<Texture2D>("fish_dart");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // put all the fish in one spritesheet
            spriteBatch.Draw(

            );
        }
    }
}
