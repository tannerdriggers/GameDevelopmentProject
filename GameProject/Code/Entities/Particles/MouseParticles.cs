using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Particles
{
    class MouseParticles : ParticleType
    {
        private Texture2D _bubblesTexture;
        private ParticleGenerator _particleGenerator;

        public override Game Game { get; set; }

        public MouseParticles(Game game)
        {
            Game = game;
        }

        public void LoadContent(ContentManager Content)
        {
            _bubblesTexture = Content.Load<Texture2D>("entities/Particle");

            _particleGenerator = new ParticleGenerator(Game, 4, _bubblesTexture,
                spawnParticle: (ref Particle? part) =>
                {
                    var particle = new Particle();
                    MouseState mouse = Mouse.GetState();
                    particle.Position = new Vector2(mouse.X, mouse.Y);
                    particle.Velocity = new Vector2(
                        MathHelper.Lerp(-50, 50, (float)random.NextDouble()), // X between -50 and 50
                        MathHelper.Lerp(0, 100, (float)random.NextDouble()) // Y between 0 and 100
                        );
                    particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                    particle.Color = Color.Gold;
                    particle.Scale = 1f;
                    particle.Life = 1.0f;
                    part = particle;
                },
                updateParticle: (float deltaT, ref Particle? part) =>
                {
                    if (part.HasValue)
                    {
                        var particle = part.Value;
                        particle.Velocity += deltaT * particle.Acceleration;
                        particle.Position += deltaT * particle.Velocity;
                        particle.Scale -= deltaT;
                        particle.Life -= deltaT;
                    }
                }
            );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _particleGenerator.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _particleGenerator.Update(gameTime);
        }
    }
}
