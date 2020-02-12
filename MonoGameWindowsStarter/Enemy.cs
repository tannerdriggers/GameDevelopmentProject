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
        public bool alive;
        TimeSpan timer;
        int frame;
        public Vector2 position;
        public EnemyType enemyType;

        Texture2D texture;

        /// <summary>
        /// Width of a single sprite in the spritesheet
        /// - Changes based on the fish sprite used
        /// </summary>
        public int FRAME_WIDTH = 30;

        /// <summary>
        /// Height of a single sprite in the spritesheet
        /// - Changes based on the fish sprite used
        /// </summary>
        public int FRAME_HEIGHT = 30;

        /// <summary>
        /// How fast the sprite animations switch
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// Speed of the enemy
        /// </summary>
        readonly float ENEMY_SPEED = 2;
        
        Random random = new Random();

        public Enemy(Game1 game)
        {
            this.game = game;
            alive = true;
            Array values = Enum.GetValues(typeof(EnemyType));
            enemyType = (EnemyType)values.GetValue(random.Next(values.Length));
            timer = new TimeSpan(0);
            position = new Vector2(game.GraphicsDevice.Viewport.Width + FRAME_WIDTH, random.Next(game.GraphicsDevice.Viewport.Height - 10));
            frame = 0;

            switch(enemyType)
            {
                case EnemyType.fish:
                    FRAME_WIDTH = 32;
                    FRAME_HEIGHT = 32;
                    texture = game.fish;
                    ENEMY_SPEED += random.Next(-1, 1) / 2f;
                    break;
                case EnemyType.fish_big:
                    FRAME_WIDTH = 54;
                    FRAME_HEIGHT = 49;
                    ENEMY_SPEED -= 1 + random.Next(-1, 1) / 2f;
                    texture = game.fish_big;
                    break;
                case EnemyType.fish_dart:
                    FRAME_WIDTH = 39;
                    FRAME_HEIGHT = 20;
                    ENEMY_SPEED += 1 + random.Next(-1, 1) / 2f;
                    texture = game.fish_dart;
                    break;
                default:
                    FRAME_WIDTH = 32;
                    FRAME_HEIGHT = 32;
                    texture = game.fish;
                    enemyType = EnemyType.fish;
                    ENEMY_SPEED += random.Next(-1, 1) / 2f;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (position.X > 0 - (FRAME_WIDTH + 5))
            {
                var randomBubbles = random.Next(100);
                if (randomBubbles == 0)
                {
                    game.bubbles.Add(new Bubble(game, position));
                }

                position.X -= ENEMY_SPEED;

                timer += gameTime.ElapsedGameTime;

                while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
                {
                    frame++;
                    timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                }

                frame %= 4;
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
                    texture: texture,
                    position: position,
                    sourceRectangle: source,
                    color: Color.White,
                    rotation: 0f,
                    origin: new Vector2(FRAME_WIDTH / 2, FRAME_HEIGHT / 2),
                    scale: 1f,
                    effects: SpriteEffects.FlipHorizontally,
                    layerDepth: 1f);
            }
        }
    }
}
