using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using GameProject.Code.Entities.Alive;

namespace GameProject.Code.Entities.Particles
{
    class ParticleGenerator : ParticleType
    {
        public override Game Game { get; set; }

        /// <summary>
        /// The collection of particles 
        /// </summary>
        Particle?[] particles;

        /// <summary>
        /// The texture this particle system uses 
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The next index in the particles array to use when spawning a particle
        /// </summary>
        int nextIndex = 0;

        public Entity entity;

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

        /*/// <summary>
        /// Constructs a new particle engine 
        /// </summary>
        /// <param name="graphicsDevice">The graphics device</param>
        /// <param name="size">The maximum number of particles in the system</param>
        /// <param name="texture">The texture of the particles</param> */


        /// <summary>
        /// Constructs a new particle engine 
        /// </summary>
        /// <param name="game">The game object</param>
        /// <param name="entity">The cooresponding entity to track to</param>
        /// <param name="size"></param>
        /// <param name="texture"></param>
        /// <param name="secondsToSkip"></param>
        /// <param name="spawnPerFrame"></param>
        public ParticleGenerator(
            Game game,
            Entity entity,
            int size,
            Texture2D texture,
            ParticleSpawner spawnParticle,
            ParticleUpdater updateParticle)
        {
            Game = game;
            this.entity = entity;
            particles = new Particle?[size];
            this.texture = texture;
            SpawnParticle = spawnParticle;
            UpdateParticle = updateParticle;
        }

        /// <summary>
        /// Constructs a new particle engine 
        /// </summary>
        /// <param name="game">The game object</param>
        /// <param name="entity">The cooresponding entity to track to</param>
        /// <param name="size"></param>
        /// <param name="texture"></param>
        /// <param name="secondsToSkip"></param>
        /// <param name="spawnPerFrame"></param>
        public ParticleGenerator(
            Game game,
            int size,
            Texture2D texture,
            ParticleSpawner spawnParticle,
            ParticleUpdater updateParticle)
        {
            Game = game;
            particles = new Particle?[size];
            this.texture = texture;
            SpawnParticle = spawnParticle;
            UpdateParticle = updateParticle;
        }

        /// <summary> 
        /// Updates the particle system, spawining new particles and 
        /// moving all live particles around the screen 
        /// </summary>
        /// <param name="gameTime">A structure representing time in the game</param>
        public override void Update(GameTime gameTime)
        {
            // Make sure our delegate properties are set
            if (SpawnParticle == null || UpdateParticle == null) return;

            // Create the particle
            SpawnParticle(ref particles[nextIndex]);

            // Advance the index 
            nextIndex++;
            if (nextIndex > particles.Length - 1) nextIndex = 0;
            
            // Part 2: Update Particles
            float deltaT = 0.5f; // (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < particles.Length; i++)
            {
                // delete any "dead" particles
                if (particles[i]?.Life <= 0)
                {
                    particles[i] = null;
                }
                else
                {
                    // Update the individual particle
                    UpdateParticle(deltaT, ref particles[i]);
                }
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.End();
            //var t = Matrix.CreateTranslation(game.worldOffset.X, game.worldOffset.Y, 0);
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, t);

            // TODO: Draw particles
            // Iterate through the particles
            for (int i = 0; i < particles.Length; i++)
            {
                // Draw the individual particles
                if (particles[i].HasValue)
                {
                    // Skip any "dead" particles
                    if (particles[i].Value.Life <= 0)
                    {
                        particles[i] = null;
                    }
                    else
                    {
                        spriteBatch.Draw(texture, particles[i].Value.Position, null, color: particles[i].Value.Color, 0f, Vector2.Zero, particles[i].Value.Scale, SpriteEffects.None, 0);
                    }
                }
            }

            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
        }
    }
}
