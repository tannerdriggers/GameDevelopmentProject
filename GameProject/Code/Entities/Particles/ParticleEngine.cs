using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Particles
{
    class ParticleEngine : ParticleType
    {
        public override Game Game { get; set; }

        private readonly List<Particle> _particles;
        private readonly List<Texture2D> _textures;
        private float _modifierCurrentTime;

        /// <summary>
        /// Number of elapsed seconds until spawning another particle
        /// DEFAULT: 0.5 seconds
        /// </summary>
        public float NumberOfSecondsPerSpawn { get; set; } = 0.5f;

        /// <summary>
        /// Acceleration of the particles
        /// </summary>
        public Vector2 ParticleAcceleration { get; set; } = Vector2.Zero;

        /// <summary>
        /// Scale of the Particles
        /// </summary>
        public float ParticleScale { get; set; } = 0.01f;

        /// <summary>
        /// Creates an Engine to spawn particles
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="textures">Textures to be randomly assigned to particles</param>
        /// <param name="emitterLocation">Location for the Emitter to be placed</param>
        public ParticleEngine(Game game, List<Texture2D> textures, Vector2 emitterLocation)
        {
            Game = game;
            _textures = textures;
            _particles = new List<Particle>();
            EmitterLocation = emitterLocation;
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[random.Next(_textures.Count)];
            float angle = 0;
            float angularVelocity = 0;
            Color color = Color.Aqua;

            return new Particle(texture, EmitterLocation, SPEED, ParticleAcceleration, angle, angularVelocity, color, ParticleScale, (int)ParticleLife);
        }

        public override void Update(GameTime gameTime)
        {
            for (int particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update();
                if (_particles[particle].Life <= 0)
                {
                    _particles.RemoveAt(particle);
                    particle--;
                }
            }

            if (EmitterLife != null)
            {
                if (EmitterLife > 0)
                {
                    var totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_modifierCurrentTime >= NumberOfSecondsPerSpawn)
                    {
                        _particles.Add(GenerateNewParticle());
                        _modifierCurrentTime = 0;
                    }
                    else
                    {
                        _modifierCurrentTime += totalSeconds;
                    }
                    EmitterLife -= totalSeconds;
                }
            }
            else
            {
                _particles.Add(GenerateNewParticle());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < _particles.Count; index++)
            {
                _particles[index].Draw(spriteBatch);
            }
        }
    }
}
