using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code
{
    class Background : Entity
    {
        private Queue<BackgroundModel> backgrounds;
        private Game Game { get; set; }
        public Texture2D backgroundImage;
        public Texture2D midgroundImage;

        public Background(Game game)
        {
            Game = game;
            backgrounds = new Queue<BackgroundModel>();
        }
        
        public void LoadContent()
        {
            backgroundImage = Game.Content.Load<Texture2D>("background");
            midgroundImage = Game.Content.Load<Texture2D>("midground");
            backgrounds.Enqueue(new BackgroundModel(new BoundingRectangle(-50, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height)));
        }

        public void Update(GameTime gameTime)
        {
            var offset = Game.player.PlayerScreenOffset;
            if (Game.player.Position.X + ((backgrounds.Count - 1) * Game.GraphicsDevice.Viewport.Width) > offset)
            {
                backgrounds.Enqueue(
                    new BackgroundModel(
                        new BoundingRectangle(
                            (backgrounds.Count - 1) * Game.GraphicsDevice.Viewport.Width - offset,
                            0,
                            Game.GraphicsDevice.Viewport.Width,
                            Game.GraphicsDevice.Viewport.Height
                        )
                    )
                );
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: backgroundImage,
                position: Game.worldOffset * -1,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: new Vector2(0, 0),
                scale: new Vector2(Game.GraphicsDevice.Viewport.Width / 250, Game.GraphicsDevice.Viewport.Height / 200),
                effects: SpriteEffects.None,
                layerDepth: 0
            );

            foreach (var background in backgrounds)
            {
                spriteBatch.Draw(
                    midgroundImage, 
                    new Rectangle(
                        (int)background.background.X, 
                        (int)background.background.Y, 
                        (int)background.background.Width, 
                        (int)background.background.Height
                    ), 
                    Color.White);
            }
        }
    }
}
