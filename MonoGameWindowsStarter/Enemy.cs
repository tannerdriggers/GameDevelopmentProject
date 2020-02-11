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
    enum EnemyType
    {
        fish,
        fish_big,
        fish_dart
    }

    class Enemy
    {
        Game1 game;
        Texture2D fish;
        Texture2D fish_big;
        Texture2D fish_dart;

        public EnemyType enemyType;
        public Vector2 position;
        public int FRAME_WIDTH = 30;
        public int FRAME_HEIGHT = 30;


        enum playerState {
            idle = 0,
            swimming = 1,
            hurt = 4
        }

        public Enemy(Game1 game)
        {
            this.game = game;
            Array values = Enum.GetValues(typeof(EnemyType));
            Random random = new Random();
            enemyType = (EnemyType)values.GetValue(random.Next(values.Length));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // put all the fish in one spritesheet
            //spriteBatch.Draw(
            //    frame * FRAME_WIDTH,
            //    new Vector2(100, 50),
            //    Color.White
            //);
        }
    }
}
