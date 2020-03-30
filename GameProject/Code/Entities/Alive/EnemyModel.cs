using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using GameProject.Code.Entities.Particles;

namespace GameProject.Code.Entities.Alive
{
    /// <summary>
    /// The different types of Enemies
    /// </summary>
    enum EnumEnemyType
    {
        fish,
        fish_big,
        fish_dart
    }

    /// <summary>
    /// Model for the Enemy
    /// </summary>
    class EnemyModel : EnemyType
    {
        public TimeSpan timer;
        public int frame;
        public List<ParticleType> ParticleEngines;

        private readonly Enemy enemyFlyweight;

        public override Game Game { get; set; }

        public EnemyModel(Game game)
        {
            ParticleEngines = new List<ParticleType>();
            enemyFlyweight = game.enemyFlyweight;
            Alive = true;
            SPEED = new Vector2(2, 0);
            Position = new Vector2(game.GraphicsDevice.Viewport.Width + FRAME_WIDTH + game.player.Position.X, enemyFlyweight.random.Next(game.GraphicsDevice.Viewport.Height - 10));

            Array values = Enum.GetValues(typeof(EnumEnemyType));
            enemyType = (EnumEnemyType)values.GetValue(enemyFlyweight.random.Next(values.Length));
            
            enemyFlyweight.SetWidthHeightSpeed(this);

            timer = new TimeSpan(0);
            hitBox = new BoundingRectangle(
                Position.X + LEFT_COLLISION_OFFSET,
                Position.Y + TOP_COLLISION_OFFSET,
                FRAME_WIDTH - RIGHT_COLLISION_OFFSET - LEFT_COLLISION_OFFSET,
                FRAME_HEIGHT - BOTTOM_COLLISION_OFFSET - TOP_COLLISION_OFFSET);

            frame = 0;
        }

        public EnemyModel(Game game, Vector2 position)
        {
            ParticleEngines = new List<ParticleType>();
            enemyFlyweight = game.enemyFlyweight;
            Alive = true;
            SPEED = Vector2.Zero;
            Position = position;

            Array values = Enum.GetValues(typeof(EnumEnemyType));
            enemyType = (EnumEnemyType)values.GetValue(enemyFlyweight.random.Next(values.Length));

            enemyFlyweight.SetWidthHeightSpeed(this);

            timer = new TimeSpan(0);
            hitBox = new BoundingRectangle(
                position.X + LEFT_COLLISION_OFFSET,
                position.Y + TOP_COLLISION_OFFSET,
                FRAME_WIDTH - RIGHT_COLLISION_OFFSET - LEFT_COLLISION_OFFSET,
                FRAME_HEIGHT - BOTTOM_COLLISION_OFFSET - TOP_COLLISION_OFFSET);

            frame = 0;
        }

        public EnemyModel(Game game, Vector2 position, EnumEnemyType enemyType)
        {
            ParticleEngines = new List<ParticleType>();
            enemyFlyweight = game.enemyFlyweight;
            Alive = true;
            SPEED = Vector2.Zero;
            Position = position;

            this.enemyType = enemyType;

            enemyFlyweight.SetWidthHeightSpeed(this);

            timer = new TimeSpan(0);
            hitBox = new BoundingRectangle(
                position.X + LEFT_COLLISION_OFFSET,
                position.Y + TOP_COLLISION_OFFSET,
                FRAME_WIDTH - RIGHT_COLLISION_OFFSET - LEFT_COLLISION_OFFSET,
                FRAME_HEIGHT - BOTTOM_COLLISION_OFFSET - TOP_COLLISION_OFFSET);

            frame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
