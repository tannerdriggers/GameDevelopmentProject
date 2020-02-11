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
    enum playerState {
        idle = 0,
        swimming = 1,
        hurt = 4
    }

    class Player
    {
        Game1 game;
        Texture2D playerSpriteSheet;
        playerState playerState;
        TimeSpan timer;
        Vector2 position;
        int frame;

        /// <summary>
        /// Whether to flip the player sprite or not
        /// </summary>
        bool flipPlayer = false;

        /// <summary>
        /// Speed of the player
        /// </summary>
        const float PLAYER_SPEED = 200;

        /// <summary>
        /// Width of a single sprite in the spritesheet
        /// </summary>
        const int FRAME_WIDTH = 80;

        /// <summary>
        /// Height of a single sprite in the spritesheet
        /// </summary>
        const int FRAME_HEIGHT = 80;

        /// <summary>
        /// How fast the sprite animations switch
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        public Player(Game1 game)
        {
            this.game = game;
            playerState = playerState.swimming;
            timer = new TimeSpan(0);
            position = new Vector2(200, 200);
            frame = 0;
        }

        public void LoadContent(ContentManager Content)
        {
            playerSpriteSheet = Content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboard.IsKeyDown(Keys.Down))
            {
                playerState = playerState.idle;
                position.Y += delta * PLAYER_SPEED;
                playerDirection = 2;
            }

            if (keyboard.IsKeyDown(Keys.Up))
            {
                playerState = playerState.idle;
                position.Y -= delta * PLAYER_SPEED;
                playerDirection = 1;
            }

            if (keyboard.IsKeyDown(Keys.Left))
            {
                playerState = playerState.swimming;
                position.X -= delta * PLAYER_SPEED;
                playerDirection = 3;
            }

            if (keyboard.IsKeyDown(Keys.Right))
            {
                playerState = playerState.swimming;
                position.X += delta * PLAYER_SPEED;
                playerDirection = 0;
            }

            if (!( keyboard.IsKeyDown(Keys.Right) 
                || keyboard.IsKeyDown(Keys.Left) 
                || keyboard.IsKeyDown(Keys.Up) 
                || keyboard.IsKeyDown(Keys.Down)))
            {
                playerState = playerState.idle;
                playerDirection = 0;
            }

            if (game.enemies.Exists(
                enemy =>
                {
                    return !(enemy.position.X > position.X + FRAME_WIDTH
                          || enemy.position.X + enemy.FRAME_WIDTH < position.X
                          || enemy.position.Y > position.Y + FRAME_HEIGHT
                          || enemy.position.Y + enemy.FRAME_HEIGHT < position.Y);
                })
            )
            {
                playerState = playerState.hurt;
                playerDirection = 0;
            }

            timer += gameTime.ElapsedGameTime;

            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                frame++;
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            if (playerState == playerState.idle)
                frame %= 6;
            else
                frame %= 7;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FRAME_WIDTH,
                (int)playerState * FRAME_HEIGHT,
                FRAME_WIDTH,
                FRAME_HEIGHT
            );

            spriteBatch.Draw(texture: playerSpriteSheet, position: position, sourceRectangle: source, color: Color.White);
        }
    }
}
