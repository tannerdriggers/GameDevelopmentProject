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
using GameProject.Code.Scrolling;

namespace GameProject.Code.Entities.Alive.Player
{
    /// <summary>
    /// The different states the player can be in
    /// </summary>
    public enum playerState {
        idle = 0,
        swimming = 1,
        hurt = 4
    }

    public class Player : LivingCreature
    {
        public int PlayerScreenOffset { get; } = 50;

        private Texture2D playerSpriteSheet;
        public playerState playerState;
        private TimeSpan timer;
        private int frame;
        private SpriteEffects effect;
        private SoundEffect playerDeathSound;
        private PlayerParticles _playerParticleGenerator;

        /// <summary>
        /// The angle the helicopter should tilt
        /// </summary>
        float angle = 0;

        /// <summary>
        /// How fast the player moves
        /// </summary>
        public float Speed { get; set; } = 100;

        public Player(Game game, Texture2D texture) : base(game, texture)
        {
            playerState = playerState.swimming;
            timer = new TimeSpan(0);
            Position = new Vector2(PlayerScreenOffset, (game.GraphicsDevice.Viewport.Height / 2));
            hitBox = new BoundingRectangle(Position.X, Position.Y, FRAME_WIDTH, FRAME_HEIGHT);
            frame = 0;
            effect = SpriteEffects.None;

            _playerParticleGenerator = new PlayerParticles(Game, null);

            TOP_COLLISION_OFFSET = 29;
            BOTTOM_COLLISION_OFFSET = 32;
            RIGHT_COLLISION_OFFSET = 15;
            LEFT_COLLISION_OFFSET = 15;
        }

        public ParallaxLayer LoadContent(ContentManager Content)
        {
            // Load Player Particle Generator
            _playerParticleGenerator.LoadContent(Content);

            // Load Player Death Noise
            playerDeathSound = Content.Load<SoundEffect>("entities/death");

            // Load Player
            playerSpriteSheet = Content.Load<Texture2D>("entities/player");
            var playerLayer = new ParallaxLayer(Game);
            playerLayer.Sprites.Add(this);
            playerLayer.DrawOrder = 3;
            return playerLayer;
        }

        public void UnloadContent()
        {
            if (playerSpriteSheet != null)
            {
                playerSpriteSheet.Dispose();
                playerDeathSound.Dispose();
            }
        }

        public new void Update(GameTime gameTime)
        {
            _playerParticleGenerator.Update(gameTime);
            hitBox = new BoundingRectangle(
                Position.X + LEFT_COLLISION_OFFSET,
                Position.Y + TOP_COLLISION_OFFSET,
                FRAME_WIDTH - RIGHT_COLLISION_OFFSET - LEFT_COLLISION_OFFSET,
                FRAME_HEIGHT - BOTTOM_COLLISION_OFFSET - TOP_COLLISION_OFFSET);

            // State pattern
            if (!Game.gameFinished)
            {
                Vector2 direction = Vector2.Zero;
                playerState = playerState.swimming;

                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Override keyboard
                KeyboardState keyboard = Keyboard.GetState();

                // Use GamePad for input
                var gamePad = GamePad.GetState(0);

                // The thumbstick value is a vector2 with X & Y between [-1f and 1f] and 0 if no GamePad is available
                direction.X = gamePad.ThumbSticks.Left.X;

                // We need to inverty the Y axis
                direction.Y = -gamePad.ThumbSticks.Left.Y;
#if DEBUG
                if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
                {
                    direction.X -= 1;
                    effect = SpriteEffects.None;
                }
#endif
                if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                {
                    direction.X += 1;
                    effect = SpriteEffects.None;
                }
                if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                    && hitBox.Y > 0)
                {
                    direction.Y -= 1;
                    effect = SpriteEffects.None;
                }
                if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                    && hitBox.Y + hitBox.Height < Game.GraphicsDevice.Viewport.Height)
                {
                    direction.Y += 1;
                    effect = SpriteEffects.None;
                }

                // Caclulate the tilt of the helicopter
                angle = 0.5f * direction.X;

                // Move the player
                Position += delta * Speed * direction;

                ///////////////////
//                
//                Position.X = -Game.worldOffset.X + PlayerScreenOffset;
//                playerState = playerState.swimming;

//                if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
//                    && hitBox.Y + hitBox.Height < Game.GraphicsDevice.Viewport.Height)
//                {
//                    Position.Y += delta * SPEED.Y;
//                    effect = SpriteEffects.None;
//                }

//                if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
//                    && hitBox.Y > 0)
//                {
//                    Position.Y -= delta * SPEED.Y;
//                    effect = SpriteEffects.None;
//                }

//#if DEBUG
//                // Player left
//                if ((keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A)))
//                {
//                    Game.worldOffset.X += delta * SPEED.X;
//                    effect = SpriteEffects.FlipHorizontally;
//                }
//#endif

//                // Player right
//                if ((keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)))
//                {
//                    Game.worldOffset.X -= delta * SPEED.X;
//                    effect = SpriteEffects.None;
//                }

//                //if (!(keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D)
//                //    || keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A)
//                //    || keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W)
//                //    || keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S)))
//                //{
//                //    playerState = playerState.idle;
//                //    effect = SpriteEffects.None;
//                //}

                //////////////////////////////////////////////////

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
                                && enemy.hitBox.X + enemy.hitBox.Width >= hitBox.X                            // player left side
                                && enemy.hitBox.Y <= hitBox.Y + hitBox.Height                                 // player bottom
                                && enemy.hitBox.Y + enemy.hitBox.Height >= hitBox.Y);                         // player top
                    });
        }

        public new void Draw(SpriteBatch spriteBatch)
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
