using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GameProject.Code
{
    /// <summary>
    /// The different states the player can be in
    /// </summary>
    enum playerState {
        idle = 0,
        swimming = 1,
        hurt = 4
    }

    class Player
    {
        public Vector2 position;

        private readonly Game1 game;
        private Texture2D playerSpriteSheet;
        private playerState playerState;
        private TimeSpan timer;
        private int frame;
        private SpriteEffects effect;
        private SoundEffect playerDeathSound;

        /// <summary>
        /// Speed of the player
        /// </summary>
        private const float PLAYER_SPEED = 250;

        /// <summary>
        /// Width of a single sprite in the spritesheet
        /// </summary>
        private const int FRAME_WIDTH = 80;

        /// <summary>
        /// Height of a single sprite in the spritesheet
        /// </summary>
        private const int FRAME_HEIGHT = 80;

        /// <summary>
        /// How fast the sprite animations switch
        /// </summary>
        private const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// Size of the player
        /// </summary>
        public float scale = 1f;

        private const int BOTTOM_COLLISION_OFFSET = 52;
        private const int TOP_COLLISION_OFFSET2 = 3;
        private const int RIGHT_COLLISION_OFFSET = 55;
        private const int LEFT_COLLISION_OFFSET2 = 7;

        public Player(Game1 game)
        {
            this.game = game;
            playerState = playerState.swimming;
            timer = new TimeSpan(0);
            position = new Vector2(50, (game.GraphicsDevice.Viewport.Height / 2));
            frame = 0;
            effect = SpriteEffects.None;
        }

        public void LoadContent(ContentManager Content)
        {
            playerSpriteSheet = Content.Load<Texture2D>("player");
            playerDeathSound = Content.Load<SoundEffect>("death");
        }

        public void UnloadContent()
        {
            if (playerSpriteSheet != null)
            {
                playerSpriteSheet.Dispose();
                playerDeathSound.Dispose();
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // State pattern
            if (!game.gameFinished)
            {
                if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                    && position.Y < game.GraphicsDevice.Viewport.Height - (FRAME_HEIGHT / 4) + TOP_COLLISION_OFFSET2)
                {
                    playerState = playerState.idle;
                    position.Y += delta * PLAYER_SPEED;
                    effect = SpriteEffects.None;
                }

                if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                    && position.Y - (FRAME_HEIGHT / 4) - TOP_COLLISION_OFFSET2 > 0)
                {
                    playerState = playerState.idle;
                    position.Y -= delta * PLAYER_SPEED;
                    effect = SpriteEffects.None;
                }
#if DEBUG
                if ((keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A)))
                {
                    playerState = playerState.swimming;
                    position.X -= delta * PLAYER_SPEED;
                    effect = SpriteEffects.FlipHorizontally;
                }
#endif

                if ((keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)))
                {
                    playerState = playerState.swimming;
                    position.X += delta * PLAYER_SPEED;
                    effect = SpriteEffects.None;
                }

                if (!(keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)
                    || keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A)
                    || keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)
                    || keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)))
                {
                    playerState = playerState.idle;
                    effect = SpriteEffects.None;
                }

                if (Collision())
                {
                    playerState = playerState.hurt;
                    effect = SpriteEffects.None;
                    game.gameFinished = true;
                    playerDeathSound.Play();
                }
            }
            else
            {
                if (scale > 0f)
                {
                    scale -= 0.005f;
                }
            }

            timer += gameTime.ElapsedGameTime;

            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                frame++;
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            if (playerState == playerState.idle)
                frame %= 6;
            else if (playerState == playerState.hurt)
                frame %= 5;
            else
                frame %= 7;
        }

        /// <summary>
        /// Collision using spacial partitioning
        /// </summary>
        /// <returns>If the player collides with an Enemy</returns>
        private bool Collision()
        {
            return game
                .enemyFlyweight
                .enemies
                .AsQueryable()
                .Where(enemy => enemy.position.Y < position.Y + FRAME_HEIGHT + 20 && enemy.position.Y + enemy.FRAME_HEIGHT + 20 > position.Y)
                .ToList()
                .Exists(enemy =>
                    {
                        return (enemy.position.X < position.X + FRAME_WIDTH - RIGHT_COLLISION_OFFSET                          // player right side
                                && enemy.position.X + enemy.FRAME_WIDTH > position.X + LEFT_COLLISION_OFFSET2                 // player left side
                                && enemy.position.Y < position.Y + FRAME_HEIGHT - BOTTOM_COLLISION_OFFSET                     // player bottom
                                && enemy.position.Y + enemy.FRAME_HEIGHT > position.Y - TOP_COLLISION_OFFSET2);               // player top
                    });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FRAME_WIDTH,
                (int)playerState * FRAME_HEIGHT,
                FRAME_WIDTH,
                FRAME_HEIGHT
            );

            spriteBatch.Draw(
                texture: playerSpriteSheet, 
                position: position, 
                sourceRectangle: source, 
                color: Color.White, 
                rotation: 0f, 
                origin: new Vector2(FRAME_WIDTH / 2, FRAME_HEIGHT / 2), 
                scale: scale,
                effects: effect, 
                layerDepth: 0f);
        }
    }
}
