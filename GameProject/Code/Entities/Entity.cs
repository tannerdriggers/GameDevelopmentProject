using GameProject.Code.Entities.Alive;
using GameProject.Code.Entities.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities
{
    abstract class Entity
    {
        public abstract Game Game { get; set; }

        public Vector2 Position { get; set; }

        public Random random = new Random();

        /// <summary>
        /// How fast the sprite animations switch
        /// </summary>
        public int ANIMATION_FRAME_RATE => 124;

        /// <summary>
        /// Speed of the entity
        /// </summary>
        public Vector2 SPEED = new Vector2(250, 250);

        /// <summary>
        /// Width of a single sprite in the spritesheet
        /// </summary>
        public int FRAME_WIDTH = 80;

        /// <summary>
        /// Height of a single sprite in the spritesheet
        /// </summary>
        public int FRAME_HEIGHT = 80;

        public int TOP_COLLISION_OFFSET = 0;
        public int BOTTOM_COLLISION_OFFSET = 0;
        public int RIGHT_COLLISION_OFFSET = 0;
        public int LEFT_COLLISION_OFFSET = 0;

        public BoundingRectangle hitBox;

        /// <summary>
        /// Size of the player
        /// </summary>
        public float scale = 1f;

        public void AddParticleGenerator(Entity entity, Texture2D texture, Particle? particle)
        {
            var pg = new ParticleGenerator(Game, entity, 1, texture,
                spawnParticle: (ref Particle? par) =>
                {
                    Particle tempParticle = new Particle();
                    if (particle.HasValue)
                    {
                        tempParticle = particle.Value;
                        tempParticle.Position = entity.Position;
                    }
                    par = tempParticle;
                },
                updateParticle: (float deltaT, ref Particle? par) =>
                {
                    if (par.HasValue)
                    {
                        var tempParticle = par.Value;
                        tempParticle.Velocity += deltaT * tempParticle.Acceleration;
                        tempParticle.Position += deltaT * tempParticle.Velocity;
                        tempParticle.Scale += deltaT;
                        tempParticle.Life--;
                        par = tempParticle;
                    }
                }
            );

            //Game.ParticleEngines.Add(pg);
        }

        public void AddParticleGenerator(Vector2 position, Texture2D texture, Color color, float scale, float life)
        {
            var pg = new ParticleGenerator(Game, 1, texture,
                (ref Particle? particle) =>
                {
                    var par = new Particle
                    {
                        Position = position,
                        Velocity = new Vector2(
                            MathHelper.Lerp(-50, 50, (float)random.NextDouble()), // X between -50 and 50
                            MathHelper.Lerp(0, 500, (float)random.NextDouble()) // Y between 0 and 100
                        ),
                        Acceleration = 0.3f * new Vector2(0, (float)-random.NextDouble()),
                        Color = color,
                        Scale = scale,
                        Life = life
                    };
                    particle = par;
                },
                (float deltaT, ref Particle? particle) =>
                {
                    if (particle.HasValue)
                    {
                        var par = particle.Value;
                        par.Velocity += deltaT * par.Acceleration;
                        par.Position += deltaT * par.Velocity;
                        par.Scale -= deltaT;
                        par.Life -= deltaT;
                        if (par.Life > 0) // Particle is still alive
                            particle = par;
                        else
                            particle = null; // Particle is dead
                    }
                }
            );

            //Game.ParticleEngines.Add(pg);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
