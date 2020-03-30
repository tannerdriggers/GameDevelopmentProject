using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Particles
{
    class PlayerParticles : ParticleType
    {
        public override Game Game { get; set; }

        private Texture2D _particleTexture;
        private ParticleGenerator _particleGenerator;

        public PlayerParticles(Game game)
        {
            Game = game;
        }

        public void LoadContent(ContentManager Content)
        {
            _particleTexture = Content.Load<Texture2D>("entities/bubble");

            _particleGenerator = new ParticleGenerator(Game, 1000,
                spawnParticle: (ref Particle? part) =>
                {
                    var particle = new Particle
                    {
                        Position = -Game.worldOffset + new Vector2(Game.player.PlayerScreenOffset + (Game.player.hitBox.X - Game.player.Position.X), Game.player.Position.Y + Game.player.FRAME_HEIGHT / 2),
                        Velocity = new Vector2(
                            MathHelper.Lerp(-5f, -1f, (float)random.NextDouble()),
                            MathHelper.Lerp(-1f, 1f, (float)random.NextDouble())
                        ),
                        Acceleration = new Vector2(-0.05f, 0f),
                        Color = Color.Blue,
                        Scale = MathHelper.Lerp(0.002f, 0.005f, (float)random.NextDouble()),// 0.005f + (float)(random.NextDouble() / 100);
                        Life = 50f,
                        Texture = _particleTexture
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
