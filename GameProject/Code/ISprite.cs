using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code
{
    /// <summary>
    /// Interface representing a sprite to be drawn with a SpriteBatch
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Draws the ISprite.  This method should be invoked between calls to 
        /// SpriteBatch.Begin() and SpriteBatch.End() with the supplied SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Anything extra that the sprite needs to do
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

        Vector2 Scale { get; set; }

        Vector2 Position { get; set; }

        Texture2D Texture { get; set; }

        Game Game { get; set; }

        /// <summary>
        /// One frame width of the Sprite in the spritesheet
        /// </summary>
        int FrameWidth { get; set; }

        /// <summary>
        /// One frame height of the Sprite in the spritesheet
        /// </summary>
        int FrameHeight { get; set; }
    }

    /// <summary>
    /// A class representing a texture to render with a SpriteBatch
    /// </summary>
    public class StaticSprite : ISprite
    {
        public Game Game { get; set; }

        /// <summary>
        /// The sprite's position in the game world
        /// </summary>
        public Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>
        /// The texture this sprite uses
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The sprite font if it has one
        /// </summary>
        public SpriteFont SpriteFont { get; set; }

        public Vector2 Scale { get; set; } = Vector2.One;

        public string Text { get; set; } = "";
        public int FrameWidth { get; set; } = 0;
        public int FrameHeight { get; set; } = 0;

        private bool _isspritefont = false;

        /// <summary>
        /// Creates a new static sprite
        /// </summary>
        /// <param name="texture">The texture to use</param>
        public StaticSprite(Game game, Texture2D texture)
        {
            Texture = texture;
        }

        public StaticSprite(Game game, SpriteFont spriteFont)
        {
            SpriteFont = spriteFont;
            _isspritefont = true;
        }

        /// <summary>
        /// Creates a new static sprite
        /// </summary>
        /// <param name="texture">the texture to use</param>
        /// <param name="position">the upper-left hand corner of the sprite</param>
        public StaticSprite(Game game, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        /// <summary>
        /// Creates a new static sprite
        /// </summary>
        /// <param name="texture">the texture to use</param>
        /// <param name="position">the upper-left hand corner of the sprite</param>
        public StaticSprite(Game game, Vector2 scale, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
        }

        /// <summary>
        /// Creates a new static sprite
        /// </summary>
        /// <param name="texture">the texture to use</param>
        /// <param name="position">the upper-left hand corner of the sprite</param>
        public StaticSprite(Game game, Vector2 scale, Texture2D texture)
        {
            Texture = texture;
            Scale = scale;
        }

        /// <summary>
        /// Draws the sprite using the provided SpriteBatch.  This
        /// method should be invoked between SpriteBatch.Begin() 
        /// and SpriteBatch.End() calls.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!_isspritefont)
                spriteBatch.Draw(Texture, Position, color: Color.White, scale: Scale);
            else
            {
                spriteBatch.DrawString(SpriteFont, Text, Position - new Vector2(2, 0), Color.Black);
                spriteBatch.DrawString(SpriteFont, Text, Position, Color.White);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }

    public abstract class MobileSprite : ISprite
    {
        public Game Game { get; set; }

        /// <summary>
        /// The texture this sprite uses
        /// </summary>
        public Texture2D Texture { get; set; }

        public Vector2 Scale { get; set; } = Vector2.One;

        /// <summary>
        /// Rate at which the sprite changes in the spritesheet
        /// </summary>
        public int ANIMATION_FRAME_RATE { get; } = 124;

        /// <summary>
        /// The box where collisions are calculated
        /// </summary>
        public BoundingRectangle Hitbox { get; set; } = new BoundingRectangle(0, 0, 0, 0);

        /// <summary>
        /// One frame width of the Sprite in the spritesheet
        /// </summary>
        public int FrameWidth { get; set; }

        /// <summary>
        /// One frame height of the Sprite in the spritesheet
        /// </summary>
        public int FrameHeight { get; set; }

        /// <summary>
        /// Top-left point of the Sprite
        /// </summary>
        public Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>
        /// Velocity of the Sprite<para />
        /// - DEFAULT: Vector2.Zero
        /// </summary>
        public Vector2 Speed { get; set; } = Vector2.Zero;

        public int TOP_COLLISION_OFFSET { get; set; } = 0;
        public int BOTTOM_COLLISION_OFFSET { get; set; } = 0;
        public int RIGHT_COLLISION_OFFSET { get; set; } = 0;
        public int LEFT_COLLISION_OFFSET { get; set; } = 0;

        public bool IsAlive { get; set; } = true;

        public MobileSprite(Game game, Texture2D texture)
        {
            Game = game;
            Texture = texture;
            FrameWidth = texture.Width / 7;
            FrameHeight = texture.Height / 5;
            Hitbox = new BoundingRectangle(Position.X, Position.Y, FrameWidth, FrameHeight);
        }

        /// <summary>
        /// This update moves the sprite by it's speed
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            Position += Speed;
            Hitbox.X = Position.X;
            Hitbox.Y = Position.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, color: Color.White, scale: Scale);
        }
    }
}
