using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GameProject.Code.Entities.Particles;

namespace GameProject.Code.Entities.Alive
{
    /// <summary>
    /// The different states the player can be in
    /// </summary>
    enum playerState {
        idle = 0,
        swimming = 1,
        hurt = 4
    }

    class Player : LivingCreature
    {
        public override Game Game { get; set; }
        public int PlayerScreenOffset { get; } = 50;

        private Texture2D playerSpriteSheet;
        public playerState playerState;
        private TimeSpan timer;
        private int frame;
        private SpriteEffects effect;
        private SoundEffect playerDeathSound;
        private PlayerParticles _playerParticleGenerator;

        public Player(Game game)
        {
            Game = game;
            playerState = playerState.swimming;
            timer = new TimeSpan(0);
            Position = new Vector2(PlayerScreenOffset, (game.GraphicsDevice.Viewport.Height / 2));
            hitBox = new BoundingRectangle(Position.X, Position.Y, FRAME_WIDTH, FRAME_HEIGHT);
            frame = 0;
            effect = SpriteEffects.None;

            _playerParticleGenerator = new PlayerParticles(Game);

            TOP_COLLISION_OFFSET = 29;
            BOTTOM_COLLISION_OFFSET = 32;
            RIGHT_COLLISION_OFFSET = 15;
            LEFT_COLLISION_OFFSET = 15;
        }

        public void LoadContent(ContentManager Content)
        {
            playerSpriteSheet = Content.Load<Texture2D>("entities/player");
            playerDeathSound = Content.Load<SoundEffect>("entities/death");

            _playerParticleGenerator.LoadContent(Content);
        }

        public void UnloadContent()
        {
            if (playerSpriteSheet != null)
            {
                playerSpriteSheet.Dispose();
                playerDeathSound.Dispose();
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            hitBox = new BoundingRectangle(
                Position.X + LEFT_COLLISION_OFFSET,
                Position.Y + TOP_COLLISION_OFFSET,
                FRAME_WIDTH - RIGHT_COLLISION_OFFSET - LEFT_COLLISION_OFFSET,
                FRAME_HEIGHT - BOTTOM_COLLISION_OFFSET - TOP_COLLISION_OFFSET);

            // State pattern
            if (!Game.gameFinished)
            {
                _playerParticleGenerator.Update(gameTime);
                Position.X = -Game.worldOffset.X + PlayerScreenOffset;
                playerState = playerState.swimming;

                if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                    && hitBox.Y + hitBox.Height < Game.GraphicsDevice.Viewport.Height)
                {
                    Position.Y += delta * SPEED.Y;
                    effect = SpriteEffects.None;
                }

                if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                    && hitBox.Y > 0)
                {
                    Position.Y -= delta * SPEED.Y;
                    effect = SpriteEffects.None;
                }

#if DEBUG
                // Player left
                if ((keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A)))
                {
                    Game.worldOffset.X += delta * SPEED.X;
                    effect = SpriteEffects.FlipHorizontally;
                }
#endif

                // Player right
                if ((keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)))
                {
                    Game.worldOffset.X -= delta * SPEED.X;
                    effect = SpriteEffects.None;
                }

                //if (!(keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)
                //    || keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A)
                //    || keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)
                //    || keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)))
                //{
                //    playerState = playerState.idle;
                //    effect = SpriteEffects.None;
                //}

                if (Collision())
                {
                    playerState = playerState.hurt;
                    effect = SpriteEffects.None;
                    Game.gameFinished = true;
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
            return Game
                .enemyFlyweight
                .enemies
                .AsQueryable()
                .Where(enemy => enemy.Position.Y < Position.Y + FRAME_HEIGHT + 20 && enemy.Position.Y + enemy.FRAME_HEIGHT + 20 > Position.Y)
                .ToList()
                .Exists(enemy =>
                    {
                        return (enemy.hitBox.X <= hitBox.X + hitBox.Width                                     // player right side
                                && enemy.hitBox.X + enemy.hitBox.Width >= hitBox.X                  // player left side
                                && enemy.hitBox.Y <= hitBox.Y + hitBox.Height                                 // player bottom
                                && enemy.hitBox.Y + enemy.hitBox.Height >= hitBox.Y);               // player top
                    });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FRAME_WIDTH,
                (int)playerState * FRAME_HEIGHT,
                FRAME_WIDTH,
                FRAME_HEIGHT
            );

#if DEBUG
            //VisualDebugging.DrawRectangle(
            //    spriteBatch, 
            //    playerHitbox,
            //    Color.Black);
#endif

            if (!Game.gameFinished)
                _playerParticleGenerator.Draw(spriteBatch);

            spriteBatch.Draw(
                texture: playerSpriteSheet, 
                position: Position, 
                sourceRectangle: source, 
                color: Color.White, 
                rotation: 0f, 
                origin: Vector2.Zero,
                scale: scale,
                effects: effect, 
                layerDepth: 0f);

        }
    }
}
