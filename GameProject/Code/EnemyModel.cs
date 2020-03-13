using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GameProject.Code
{
    /// <summary>
    /// The different types of Enemies
    /// </summary>
    enum EnemyType
    {
        fish,
        fish_big,
        fish_dart
    }

    /// <summary>
    /// Model for the Enemy
    /// </summary>
    class EnemyModel
    {
        public bool debug = false;

        public TimeSpan timer;
        public int frame;
        public int FRAME_WIDTH;
        public int FRAME_HEIGHT;
        public float ENEMY_SPEED;

        public bool alive;
        public Vector2 position;
        public EnemyType enemyType;
        private readonly Enemy enemyFlyweight;

        public EnemyModel(Game1 game)
        {
            enemyFlyweight = game.enemyFlyweight;
            alive = true;
            ENEMY_SPEED = 2;

            Array values = Enum.GetValues(typeof(EnemyType));
            enemyType = (EnemyType)values.GetValue(enemyFlyweight.random.Next(values.Length));
            
            enemyFlyweight.SetWidthHeightSpeed(this);

            timer = new TimeSpan(0);
            position = new Vector2(game.GraphicsDevice.Viewport.Width + FRAME_WIDTH + game.player.playerPosition.X, enemyFlyweight.random.Next(game.GraphicsDevice.Viewport.Height - 10));

            frame = 0;
        }

        public EnemyModel(Game1 game, Vector2 position)
        {
            debug = true;

            enemyFlyweight = game.enemyFlyweight;
            alive = true;
            ENEMY_SPEED = 0;

            Array values = Enum.GetValues(typeof(EnemyType));
            enemyType = (EnemyType)values.GetValue(enemyFlyweight.random.Next(values.Length));

            enemyFlyweight.SetWidthHeightSpeed(this);

            timer = new TimeSpan(0);
            this.position = position;

            frame = 0;
        }
    }
}
