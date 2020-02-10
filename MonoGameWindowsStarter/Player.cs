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
    class Player
    {
        Game1 game;
        Texture2D playerSpriteSheet;

        enum playerState {
            idle = 0,
            swimming = 1,
            hurt = 4
        }

        public Player(Game1 game)
        {
            this.game = game;
        }

        public void LoadContent(ContentManager Content)
        {
            playerSpriteSheet = Content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                playerSpriteSheet,

            );
        }
    }
}
