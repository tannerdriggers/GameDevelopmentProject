using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code
{
    /// <summary>
    /// The different types of Enemies
    /// </summary>
    public enum EnumEnemyType
    {
        fish,
        fish_big,
        fish_dart
    }

    public class Enemy : MobileSprite
    {
        public EnumEnemyType EnemyType { get; set; } = EnumEnemyType.fish;

        private TimeSpan timer = new TimeSpan(0);
        private int frame = 0;
        private Random random = new Random();

        public Enemy(Game game, EnumEnemyType enemyType, Texture2D texture, Vector2 position) : base(game, texture)
        {
            Position = position;
            EnemyType = enemyType;
            Speed = new Vector2(0, 0);
            SetWidthHeightSpeed(enemyType);
        }

        public override void Update(GameTime gameTime)
        {
            if (Position.X + FrameWidth < Game.player.Position.X - ((PlayerTrackingScrollController)Game.playerLayer.ScrollController).Offset)
            {
                IsAlive = false;
            }

            timer += gameTime.ElapsedGameTime;

            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                frame++;
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }
            
            frame %= 4;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(
                frame * FrameWidth,
                0,
                FrameWidth,
                FrameHeight
            );

            spriteBatch.Draw(
                texture: Texture,
                position: Position,
                sourceRectangle: source,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1f,
                effects: SpriteEffects.FlipHorizontally,
                layerDepth: 1f);
        }

        /// <summary>
        /// Sets the Width, Height, and Speed of the enemyModel given based on which type it is
        /// </summary>
        /// <param name="enemy">EnemyModel to set</param>
        public void SetWidthHeightSpeed(EnumEnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnumEnemyType.fish:
                    FrameWidth = 32;
                    FrameHeight = 32;
                    Speed += new Vector2(random.Next(-1, 1) / 2f, 0f);
                    TOP_COLLISION_OFFSET = 11;
                    BOTTOM_COLLISION_OFFSET = 6;
                    LEFT_COLLISION_OFFSET = 4;
                    RIGHT_COLLISION_OFFSET = 2;
                    break;
                case EnumEnemyType.fish_big:
                    FrameWidth = 54;
                    FrameHeight = 49;
                    Speed -= new Vector2(1 + random.Next(-1, 1) / 2f, 0f);
                    TOP_COLLISION_OFFSET = 11;
                    BOTTOM_COLLISION_OFFSET = 6;
                    LEFT_COLLISION_OFFSET = 5;
                    RIGHT_COLLISION_OFFSET = 5;
                    break;
                case EnumEnemyType.fish_dart:
                    FrameWidth = 39;
                    FrameHeight = 20;
                    Speed += new Vector2(1 + random.Next(-1, 1) / 2f, 0f);
                    TOP_COLLISION_OFFSET = 4;
                    BOTTOM_COLLISION_OFFSET = 4;
                    LEFT_COLLISION_OFFSET = 3;
                    RIGHT_COLLISION_OFFSET = 2;
                    break;
                default:
                    enemyType = EnumEnemyType.fish;
                    FrameWidth = 32;
                    FrameHeight = 32;
                    Speed += new Vector2(random.Next(-1, 1) / 2f, 0f);
                    TOP_COLLISION_OFFSET = 10;
                    BOTTOM_COLLISION_OFFSET = 10;
                    LEFT_COLLISION_OFFSET = 10;
                    RIGHT_COLLISION_OFFSET = 10;
                    break;
            }
        }
    }
}
