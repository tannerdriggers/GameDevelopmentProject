using GameProject.Code.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities
{
    /// <summary>
    /// Enemy Flyweight
    /// </summary>
    class Enemy
    {
        private List<ParticleGenerator> particleGenerators;

        public Texture2D fish;
        public Texture2D fish_dart;
        public Texture2D fish_big;
        public Texture2D bubblesTexture;

        /// <summary>
        /// How fast the sprite animations switch
        /// </summary>
        public int ANIMATION_FRAME_RATE => 124;

        public Random random = new Random();

        public List<EnemyModel> enemies;

        public Game game;

        public Enemy(Game game)
        {
            this.game = game;
            enemies = new List<EnemyModel>();
            particleGenerators = new List<ParticleGenerator>();
        }

        /// <summary>
        /// Adds an Enemy to the list of Enemies
        /// in ascending order
        /// </summary>
        /// <param name="enemy">EnemyModel to add</param>
        public void AddEnemy(EnemyModel enemy)
        {
            enemies.Add(enemy);
            game.TimSort(enemies, 32);
        }

        public void LoadContent(ContentManager Content)
        {
            fish = Content.Load<Texture2D>("entities/fish");
            fish_big = Content.Load<Texture2D>("entities/fish-big");
            fish_dart = Content.Load<Texture2D>("entities/fish-dart");
            bubblesTexture = Content.Load<Texture2D>("entities/bubbles");
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
        /// <param name="enemyModel">EnemyModel to set</param>
        public void SetWidthHeightSpeed(EnemyModel enemyModel)
        {
            switch (enemyModel.enemyType)
            {
                case EnemyType.fish:
                    enemyModel.FRAME_WIDTH = 32;
                    enemyModel.FRAME_HEIGHT = 32;
                    enemyModel.ENEMY_SPEED += random.Next(-1, 1) / 2f;
                    enemyModel.TOP_COLLISION_OFFSET = 11;
                    enemyModel.BOTTOM_COLLISION_OFFSET = 6;
                    enemyModel.LEFT_COLLISION_OFFSET = 4;
                    enemyModel.RIGHT_COLLISION_OFFSET = 2;
                    break;
                case EnemyType.fish_big:
                    enemyModel.FRAME_WIDTH = 54;
                    enemyModel.FRAME_HEIGHT = 49;
                    enemyModel.ENEMY_SPEED -= 1 + random.Next(-1, 1) / 2f;
                    enemyModel.TOP_COLLISION_OFFSET = 11;
                    enemyModel.BOTTOM_COLLISION_OFFSET = 6;
                    enemyModel.LEFT_COLLISION_OFFSET = 5;
                    enemyModel.RIGHT_COLLISION_OFFSET = 5;
                    break;
                case EnemyType.fish_dart:
                    enemyModel.FRAME_WIDTH = 39;
                    enemyModel.FRAME_HEIGHT = 20;
                    enemyModel.ENEMY_SPEED += 1 + random.Next(-1, 1) / 2f;
                    enemyModel.TOP_COLLISION_OFFSET = 4;
                    enemyModel.BOTTOM_COLLISION_OFFSET = 4;
                    enemyModel.LEFT_COLLISION_OFFSET = 3;
                    enemyModel.RIGHT_COLLISION_OFFSET = 2;
                    break;
                default:
                    enemyModel.enemyType = EnemyType.fish;
                    enemyModel.FRAME_WIDTH = 32;
                    enemyModel.FRAME_HEIGHT = 32;
                    enemyModel.ENEMY_SPEED += random.Next(-1, 1) / 2f;
                    enemyModel.TOP_COLLISION_OFFSET = 10;
                    enemyModel.BOTTOM_COLLISION_OFFSET = 10;
                    enemyModel.LEFT_COLLISION_OFFSET = 10;
                    enemyModel.RIGHT_COLLISION_OFFSET = 10;
                    break;
            }
        }

        /// <summary>
        /// Gets the Texture2D for the enemy type given
        /// </summary>
        /// <param name="enemyType">Enum type for enemies</param>
        /// <returns>The Texture2D for the given enemy type</returns>
        public Texture2D GetTexture2D(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.fish:
                    return fish;
                case EnemyType.fish_big:
                    return fish_big;
                case EnemyType.fish_dart:
                    return fish_dart;
                default:
                    return fish;
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];

                enemy.hitBox = new BoundingRectangle(
                    enemy.position.X + enemy.LEFT_COLLISION_OFFSET,
                    enemy.position.Y + enemy.TOP_COLLISION_OFFSET,
                    enemy.FRAME_WIDTH - enemy.RIGHT_COLLISION_OFFSET - enemy.LEFT_COLLISION_OFFSET,
                    enemy.FRAME_HEIGHT - enemy.BOTTOM_COLLISION_OFFSET - enemy.TOP_COLLISION_OFFSET);

                if (enemy.position.X > -game.worldOffset.X - (enemy.FRAME_WIDTH + 5))
                {
                    var randomBubbles = random.Next(0);
                    if (randomBubbles == 0)
                    {
                        var pg = new ParticleGenerator(game.GraphicsDevice, 1000, bubblesTexture)
                        {
                            SpawnParticle = (ref Particle particle) =>
                            {
                                particle.Position = enemy.position;
                                particle.Velocity = new Vector2(
                                    MathHelper.Lerp(-25, 25, (float)random.NextDouble()),
                                    MathHelper.Lerp(-200, 0, (float)random.NextDouble())
                                    );
                            },

                            UpdateParticle = (float deltaT, ref Particle particle) =>
                            {
                                particle.Velocity += deltaT * particle.Acceleration;
                                particle.Position += deltaT * particle.Velocity;
                                particle.Scale -= deltaT;
                                particle.Life -= deltaT;
                            },

                            Emitter = new Vector2(100, 100),
                            SpawnPerFrame = 4
                        };

                        particleGenerators.Add(pg);
                    }

                    enemy.position.X -= enemy.ENEMY_SPEED;

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
                    enemy.alive = false;
                    game.score++;
                }
            }

            particleGenerators.ForEach(particleGenerator =>
            {
                particleGenerator.Update(gameTime);
            });

            enemies = enemies.Where(enemy => enemy.alive == true).ToList();
        }

        public void Draw(SpriteBatch spriteBatch)
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
                VisualDebugging.DrawRectangle(
                    spriteBatch,
                    enemy.hitBox,
                    Color.Pink);
#endif

                spriteBatch.Draw(
                    texture: GetTexture2D(enemy.enemyType),
                    position: enemy.position,
                    sourceRectangle: source,
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1f,
                    effects: SpriteEffects.FlipHorizontally,
                    layerDepth: 1f);

            });

            particleGenerators.ForEach(particleGenerator =>
            {
                particleGenerator.Draw();
            });
        }
    }
}
