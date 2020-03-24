using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using GameProject.Code.Particles;

namespace GameProject.Code.Entities
{
    /// <summary>
    /// Bubble Flyweight
    /// </summary>
    class Bubble
    {
        Game game;

        Random random = new Random();

        public Texture2D bubblesTexture;
        public SoundEffect bubblesSound;

        /// <summary>
        /// Width of a single sprite in the spritesheet
        /// - Changes based on the fish sprite used
        /// </summary>
        public int FRAME_WIDTH = 23;

        /// <summary>
        /// Height of a single sprite in the spritesheet
        /// - Changes based on the fish sprite used
        /// </summary>
        public int FRAME_HEIGHT = 40;

        /// <summary>
        /// How fast the sprite animations switch
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;

        public List<BubbleModel> bubbles;

        ParticleGenerator pg;

        public Bubble(Game game)
        {
            this.game = game;
            bubbles = new List<BubbleModel>();
        }

        public void LoadContent(ContentManager Content)
        {
            bubblesTexture = Content.Load<Texture2D>("entities/bubbles");
            bubblesSound = Content.Load<SoundEffect>("entities/Large Bubble");

            // testing
            pg = new ParticleGenerator(game.GraphicsDevice, 1000, bubblesTexture);
            pg.SpawnParticle = (ref Particle particle) =>
            {
                var enemies = game.enemyFlyweight.enemies;
                foreach (var enemy in enemies)
                {
                    particle.Position = enemy.position;
                    particle.Velocity = new Vector2(
                        MathHelper.Lerp(-50, 50, (float)random.NextDouble()),
                        MathHelper.Lerp(0, 100, (float)random.NextDouble())
                        );
                }
            };

            pg.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };
            pg.Emitter = new Vector2(100, 100);
            pg.SpawnPerFrame = 4;
        }

        public void UnloadContent()
        {
            if (bubblesTexture != null)
            {
                bubblesTexture.Dispose();
                bubblesSound.Dispose();
            }
        }

        public void Update(GameTime gameTime)
        {
            pg.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pg.Draw();

            //Rectangle source;
            //game.bubbleFlyweight.bubbles.ForEach(bubble =>
            //{
            //    source = new Rectangle(
            //        bubble.frame * FRAME_WIDTH,
            //        0,
            //        FRAME_WIDTH,
            //        FRAME_HEIGHT
            //    );

            //    spriteBatch.Draw(
            //        texture: bubblesTexture, 
            //        position: bubble.position, 
            //        sourceRectangle: source, 
            //        color: Color.White
            //    );
            //});
        }
    }
}
