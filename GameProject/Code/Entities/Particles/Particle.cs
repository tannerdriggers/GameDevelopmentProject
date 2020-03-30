using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Code.Entities.Particles
{
    public struct Particle
    {
        /// <summary>
        /// The current position of the particle
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The current velocity of the particle
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// The current acceleration of the particle
        /// </summary>
        public Vector2 Acceleration;

        /// <summary>
        /// The current scale of the particle
        /// </summary>
        public float Scale;

        /// <summary>
        /// The current life of the particle
        /// </summary>
        public float Life;

        /// <summary>
        /// The current color of the particle
        /// </summary>
        public Color Color;

        /// <summary>
        /// The current angle of rotation of the particle
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// The speed that the angle is changing
        /// </summary>
        public float AngularVelocity { get; set; }

        public Texture2D Texture { get; set; }

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, Vector2 acceleration,
            float angle, float angularVelocity, Color color, float scale, int life)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Scale = scale;
            Life = life;
        }

        public void Update()
        {
            Life--;
            Velocity += Acceleration;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 5, Texture.Height / 5);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                Angle, origin, Scale, SpriteEffects.None, 0f);
        }
    }
}
