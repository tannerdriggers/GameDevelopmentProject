using GameProject.Code.Scrolling;
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
        private Texture2D _particleTexture;
        private ParticleGenerator _particleGenerator;

        public MouseParticles(Game game, Texture2D texture) : base(game, texture)
        {
        }

        public ParallaxLayer LoadContent(ContentManager Content)
        {
            _particleTexture = Content.Load<Texture2D>("entities/Particle");

            _particleGenerator = new ParticleGenerator(Game, 100,
                spawnParticle: (ref Particle? part) =>
                {
                    var particle = new Particle();
                    MouseState mouse = Mouse.GetState();
                    particle.Position = new Vector2(Game.player.Position.X - Game.player.PlayerScreenOffset + mouse.Position.X, mouse.Position.Y);
                    particle.Velocity = new Vector2(
                        MathHelper.Lerp(-20, 20, (float)random.NextDouble()), // X between -50 and 50
                        MathHelper.Lerp(-20, 20, (float)random.NextDouble()) // Y between 0 and 100
                        );
                    particle.Acceleration = 0.01f * new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                    particle.Color = Color.White;
                    particle.Scale = 1.5f;
                    particle.Life = 1.0f;
                    particle.Texture = _particleTexture;
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
                        part = particle;
                    }
                }
            );

            var particleSprite = new ParallaxLayer(Game);
            particleSprite.Sprites.Add(this);
            particleSprite.DrawOrder = 5;
            return particleSprite;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            var t = Matrix.CreateTranslation(Game.worldOffset.X, Game.worldOffset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, t);
            _particleGenerator.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);
        }

        public new void Update(GameTime gameTime)
        {
            _particleGenerator.Update(gameTime);
        }
    }
}
