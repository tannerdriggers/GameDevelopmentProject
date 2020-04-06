using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GameProject.Code.Scrolling;

namespace GameProject.Code
{
    /// <summary>
    /// The different states the player can be in
    /// </summary>
    public enum EnumPlayerState {
        idle = 0,
        swimming = 1,
        hurt = 4
    }

    public class Player : MobileSprite
    {
        public int ScreenOffset { get; } = 50;

        public EnumPlayerState playerState;
        private TimeSpan timer;
        private int frame;
        private SpriteEffects effect;
        private float playerSpeed;

        public Player(Game game, Texture2D texture) : base(game, texture)
        {
            playerState = EnumPlayerState.swimming;
            timer = new TimeSpan(0);
            Position = new Vector2(ScreenOffset, (game.GraphicsDevice.Viewport.Height / 2));
            FrameWidth = texture.Width / 7;
            FrameHeight = texture.Height / 5;
            Hitbox = new BoundingRectangle(Position.X, Position.Y, FrameWidth, FrameHeight);
            frame = 0;
            effect = SpriteEffects.None;
            playerSpeed = 200f;

            TOP_COLLISION_OFFSET = 29;
            BOTTOM_COLLISION_OFFSET = 32;
            RIGHT_COLLISION_OFFSET = 15;
            LEFT_COLLISION_OFFSET = 15;
        }

        public override void Update(GameTime gameTime)
        {
            Hitbox = new BoundingRectangle(
                Position.X + LEFT_COLLISION_OFFSET,
                Position.Y + TOP_COLLISION_OFFSET,
                FrameWidth - RIGHT_COLLISION_OFFSET - LEFT_COLLISION_OFFSET,
                FrameHeight - BOTTOM_COLLISION_OFFSET - TOP_COLLISION_OFFSET);

            Vector2 direction = Vector2.Zero;
            playerState = EnumPlayerState.swimming;

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
                direction.X -= playerSpeed;
                effect = SpriteEffects.None;
            }

            if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
            {
                direction.X += playerSpeed;
                effect = SpriteEffects.None;
            }
#endif

            if ((keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                && Hitbox.Y > 0)
            {
                direction.Y -= playerSpeed;
                effect = SpriteEffects.None;
            }
            if ((keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
                && Hitbox.Y + Hitbox.Height < Game.GraphicsDevice.Viewport.Height)
            {
                direction.Y += playerSpeed;
                effect = SpriteEffects.None;
            }

            // Move the player
            Position += delta * direction;

            // Adds to the player's speed the farther they go
            Speed += new Vector2(((int)Position.X) % 200 == 0 ? 0.2f : 0, 0);

            if (!Game.gameFinished && Collision())
            {
                playerState = EnumPlayerState.hurt;
                effect = SpriteEffects.None;
                Game.gameFinished = true;
            }

            timer += gameTime.ElapsedGameTime;

            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                frame++;
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            if (playerState == EnumPlayerState.idle)
                frame %= 6;
            else if (playerState == EnumPlayerState.hurt)
                frame %= 5;
            else
                frame %= 7;

            base.Update(gameTime);
        }

        /// <summary>
        /// Collision using spacial partitioning
        /// </summary>
        /// <returns>If the player collides with an Enemy</returns>
        private bool Collision()
        {
            return Game
                .enemyLayer
                .Sprites
                .AsQueryable()
                .Where(enemy => enemy.Position.Y < Position.Y + FrameHeight + 20 && enemy.Position.Y + (enemy.FrameHeight) + 20 > Position.Y)
                .ToList()
                .Exists(e =>
                    {
                        var enemy = (Enemy)e;
                        return (enemy.Hitbox.X <= Hitbox.X + Hitbox.Width                                     // player right side
                                && enemy.Hitbox.X + enemy.Hitbox.Width >= Hitbox.X                            // player left side
                                && enemy.Hitbox.Y <= Hitbox.Y + Hitbox.Height                                 // player bottom
                                && enemy.Hitbox.Y + enemy.Hitbox.Height >= Hitbox.Y);                         // player top
                    });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FrameWidth,
                (int)playerState * FrameHeight,
                FrameWidth,
                FrameHeight
            );

#if false
            VisualDebugging.DrawRectangle(
                spriteBatch,
                Hitbox,
                Color.Black);
#endif

            spriteBatch.Draw(
                texture: Texture, 
                position: Position, 
                sourceRectangle: source, 
                color: Color.White, 
                rotation: 0f, 
                origin: Vector2.Zero,
                scale: Scale,
                effects: effect, 
                layerDepth: 0f);

        }
    }
}
