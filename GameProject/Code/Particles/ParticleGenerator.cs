using GameProject.Code.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Particles
{
    class ParticleGenerator
    {
        Game game;

        /// <summary>
        /// The collection of particles 
        /// </summary>
        Particle?[] particles;

        /// <summary>
        /// The texture this particle system uses 
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The SpriteBatch this particle system uses
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// A random number generator used by the system 
        /// </summary>
        Random random = new Random();

        /// <summary>
        /// The next index in the particles array to use when spawning a particle
        /// </summary>
        int nextIndex = 0;

        public EnemyModel enemy;

        /// <summary>
        /// The rate of particle spawning 
        /// </summary>
        public int SpawnPerFrame { get; set; }

        /// <summary>
        /// A delegate for spawning particles
        /// </summary>
        /// <param name="particle">The particle to spawn</param>
        public delegate void ParticleSpawner(ref Particle? particle);

        /// <summary>
        /// A delegate for updating particles
        /// </summary>
        /// <param name="deltaT">The seconds elapsed between frames</param>
        /// <param name="particle">The particle to update</param>
        public delegate void ParticleUpdater(float deltaT, ref Particle? particle);

        /// <summary>
        /// Holds a delegate to use when spawning a new particle
        /// </summary>
        public ParticleSpawner SpawnParticle { get; set; }

        /// <summary>
        /// Holds a delegate to use when updating a particle 
        /// </summary>
        /// <param name="particle"></param>
        public ParticleUpdater UpdateParticle { get; set; }

        /// <summary>
        /// Constructs a new particle engine 
        /// </summary>
        /// <param name="graphicsDevice">The graphics device</param>
        /// <param name="size">The maximum number of particles in the system</param>
        /// <param name="texture">The texture of the particles</param> 
        public ParticleGenerator(Game game, int size, Texture2D texture, EnemyModel enemy)
        {
            this.game = game;
            this.enemy = enemy;
            particles = new Particle?[size];
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.texture = texture;
        }

        /// <summary> 
        /// Updates the particle system, spawining new particles and 
        /// moving all live particles around the screen 
        /// </summary>
        /// <param name="gameTime">A structure representing time in the game</param>
        public void Update(GameTime gameTime)
        {
            // Make sure our delegate properties are set
            if (SpawnParticle == null || UpdateParticle == null) return;

            // Part 1: Spawn new particles 
            for (int i = 0; i < SpawnPerFrame; i++)
            {
                // Create the particle
                SpawnParticle(ref particles[nextIndex] );

                // Advance the index 
                nextIndex++;
                if (nextIndex > particles.Length - 1) nextIndex = 0;
            }

            // Part 2: Update Particles
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < particles.Length; i++)
            {
                // Skip any "dead" particles
                if (particles[i]?.Life <= 0) continue;

                // Update the individual particle
                UpdateParticle(deltaT, ref particles[i]);
            }
        }

        public void Remove()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = null;
            }
            particles = null;
        }

        /// <summary>
        /// Draw the active particles in the particle system
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            var t = Matrix.CreateTranslation(game.worldOffset.X, game.worldOffset.Y, 0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, t);

            // TODO: Draw particles
            // Iterate through the particles
            for (int i = 0; i < particles.Length; i++)
            {
                // Draw the individual particles
                if (particles[i].HasValue)
                {
                    // Skip any "dead" particles
                    if (particles[i].Value.Life <= 0) continue;

                    spriteBatch.Draw(texture, particles[i].Value.Position, null, color: particles[i].Value.Color, 0f, Vector2.Zero, particles[i].Value.Scale, SpriteEffects.None, 0);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
        }
    }
}
