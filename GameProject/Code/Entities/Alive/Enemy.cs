using GameProject.Code.Entities.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Alive
{
    /// <summary>
    /// Enemy Flyweight
    /// </summary>
    class Enemy : LivingCreature
    {
        public Texture2D fish;
        public Texture2D fish_dart;
        public Texture2D fish_big;
        public Texture2D bubblesTexture;

        public List<EnemyModel> enemies;

        public override Game Game { get; set; }

        public Enemy(Game game)
        {
            Game = game;
            enemies = new List<EnemyModel>();
        }

        /// <summary>
        /// Adds an Enemy to the list of Enemies
        /// in ascending order
        /// </summary>
        /// <param name="enemy">EnemyModel to add</param>
        public void AddEnemy(EnemyModel enemy)
        {
            enemies.Add(enemy);
            //AddBubbles(enemy);
            Game.TimSort(enemies, 32);
        }

        public void AddBubbles(EnemyModel enemy)
        {
            var pg = new ParticleGenerator(Game, 5,
                spawnParticle: (ref Particle? part) =>
                {
                    var particle = new Particle
                    {
                        Position = enemy.Position + new Vector2(-5f, enemy.FRAME_HEIGHT / 2),
                        Velocity = new Vector2(
                        0f,
                        MathHelper.Lerp(-1f, -0.1f, (float)random.NextDouble())
                        ),
                        Acceleration = Vector2.Zero,
                        Color = Color.Aqua,
                        Scale = MathHelper.Lerp(0.004f, 0.015f, (float)random.NextDouble()),// 0.005f + (float)(random.NextDouble() / 100);
                        Life = 100f + (float)(random.NextDouble() * 25),
                        Texture = bubblesTexture
                    };

                    part = particle;
                },
                updateParticle: (float deltaT, ref Particle? part) =>
                {
                    if (part.HasValue)
                    {
                        var particle = part.Value;
                        particle.Velocity += particle.Acceleration;
                        particle.Position += particle.Velocity;
                        particle.Life -= deltaT;
                        part = particle;
                    }
                }
            )
            { Life = 10f };

            enemy.ParticleEngines.Add(pg);
        }

        public void LoadContent(ContentManager Content)
        {
            fish = Content.Load<Texture2D>("entities/fish");
            fish_big = Content.Load<Texture2D>("entities/fish-big");
            fish_dart = Content.Load<Texture2D>("entities/fish-dart");
            bubblesTexture = Content.Load<Texture2D>("entities/bubble");
        }

        public void UnloadContent()
        {
            if (fish != null)
            {
                fish.Dispose();
                fish_big.Dispose();
                fish_dart.Dispose();
            }
        }

        /// <summary>
        /// Sets the Width, Height, and Speed of the enemyModel given based on which type it is
        /// </summary>
        /// <param name="enemy">EnemyModel to set</param>
        public void SetWidthHeightSpeed(Entity entity)
        {
            var enemy = (EnemyModel)entity;
            switch (enemy.enemyType)
            {
                case EnumEnemyType.fish:
                    enemy.FRAME_WIDTH = 32;
                    enemy.FRAME_HEIGHT = 32;
                    enemy.SPEED += new Vector2(random.Next(-1, 1) / 2f, 0f);
                    enemy.TOP_COLLISION_OFFSET = 11;
                    enemy.BOTTOM_COLLISION_OFFSET = 6;
                    enemy.LEFT_COLLISION_OFFSET = 4;
                    enemy.RIGHT_COLLISION_OFFSET = 2;
                    break;
                case EnumEnemyType.fish_big:
                    enemy.FRAME_WIDTH = 54;
                    enemy.FRAME_HEIGHT = 49;
                    enemy.SPEED -= new Vector2(1 + random.Next(-1, 1) / 2f, 0f);
                    enemy.TOP_COLLISION_OFFSET = 11;
                    enemy.BOTTOM_COLLISION_OFFSET = 6;
                    enemy.LEFT_COLLISION_OFFSET = 5;
                    enemy.RIGHT_COLLISION_OFFSET = 5;
                    break;
                case EnumEnemyType.fish_dart:
                    enemy.FRAME_WIDTH = 39;
                    enemy.FRAME_HEIGHT = 20;
                    enemy.SPEED += new Vector2(1 + random.Next(-1, 1) / 2f, 0f);
                    enemy.TOP_COLLISION_OFFSET = 4;
                    enemy.BOTTOM_COLLISION_OFFSET = 4;
                    enemy.LEFT_COLLISION_OFFSET = 3;
                    enemy.RIGHT_COLLISION_OFFSET = 2;
                    break;
                default:
                    enemy.enemyType = EnumEnemyType.fish;
                    enemy.FRAME_WIDTH = 32;
                    enemy.FRAME_HEIGHT = 32;
                    enemy.SPEED += new Vector2(random.Next(-1, 1) / 2f, 0f);
                    enemy.TOP_COLLISION_OFFSET = 10;
                    enemy.BOTTOM_COLLISION_OFFSET = 10;
                    enemy.LEFT_COLLISION_OFFSET = 10;
                    enemy.RIGHT_COLLISION_OFFSET = 10;
                    break;
            }
        }

        /// <summary>
        /// Gets the Texture2D for the enemy type given
        /// </summary>
        /// <param name="enemyType">Enum type for enemies</param>
        /// <returns>The Texture2D for the given enemy type</returns>
        public Texture2D GetTexture2D(EnumEnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnumEnemyType.fish:
                    return fish;
                case EnumEnemyType.fish_big:
                    return fish_big;
                case EnumEnemyType.fish_dart:
                    return fish_dart;
                default:
                    return fish;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];

                enemy.hitBox = new BoundingRectangle(
                    enemy.Position.X + enemy.LEFT_COLLISION_OFFSET,
                    enemy.Position.Y + enemy.TOP_COLLISION_OFFSET,
                    enemy.FRAME_WIDTH - enemy.RIGHT_COLLISION_OFFSET - enemy.LEFT_COLLISION_OFFSET,
                    enemy.FRAME_HEIGHT - enemy.BOTTOM_COLLISION_OFFSET - enemy.TOP_COLLISION_OFFSET);

                if (enemy.ParticleEngines != null)
                {
                    // update particle engines
                    enemy.ParticleEngines.ForEach(particleEngine =>
                    {
                        particleEngine.EmitterLocation = enemy.Position;
                        particleEngine.Update(gameTime);
                    });
                }

                if (enemy.Position.X > -Game.worldOffset.X - (enemy.FRAME_WIDTH + 5))
                {
                    var randomBubbles = random.Next(200);
                    if (randomBubbles == 0)
                    {
                        AddBubbles(enemy);
                    }

                    enemy.Position -= enemy.SPEED;

                    enemy.timer += gameTime.ElapsedGameTime;

                    while (enemy.timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
                    {
                        enemy.frame++;
                        enemy.timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
                    }

                    enemy.frame %= 4;
                }
                else
                {
                    enemy.Alive = false;
                    Game.score++;
                }
            }

            enemies = enemies.Where(enemy => enemy.Alive == true).ToList();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source;
            enemies.ForEach(enemy =>
            {
                source = new Rectangle(
                    enemy.frame * enemy.FRAME_WIDTH,
                    0,
                    enemy.FRAME_WIDTH,
                    enemy.FRAME_HEIGHT
                );

#if DEBUG
                //VisualDebugging.DrawRectangle(
                //    spriteBatch,
                //    enemy.hitBox,
                //    Color.Pink);
#endif

                spriteBatch.Draw(
                    texture: GetTexture2D(enemy.enemyType),
                    position: enemy.Position,
                    sourceRectangle: source,
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f,
                    effects: SpriteEffects.FlipHorizontally,
                    layerDepth: 1f);

                enemy.ParticleEngines.ForEach(particleEngine =>
                {
                    particleEngine.Draw(spriteBatch);
                });
            });
        }
    }
}
