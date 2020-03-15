using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code
{
    class Background
    {
        private Queue<BackgroundModel> backgrounds;
        private Game game;
        public Texture2D backgroundImage;
        public Texture2D midgroundImage;

        public Background(Game game)
        {
            this.game = game;
            backgrounds = new Queue<BackgroundModel>();
        }

        public void LoadContent()
        {
            backgroundImage = game.Content.Load<Texture2D>("background");
            midgroundImage = game.Content.Load<Texture2D>("midground");
            backgrounds.Enqueue(new BackgroundModel(new BoundingRectangle(-50, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height)));
        }

        public void Update(GameTime gameTime)
        {
            if (game.player.playerPosition.X + ((backgrounds.Count - 1) * game.GraphicsDevice.Viewport.Width) > 50)
            {
                backgrounds.Enqueue(
                    new BackgroundModel(
                        new BoundingRectangle(
                            (backgrounds.Count - 1) * game.GraphicsDevice.Viewport.Width - 50,
                            0,
                            game.GraphicsDevice.Viewport.Width,
                            game.GraphicsDevice.Viewport.Height
                        )
                    )
                );
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: backgroundImage,
                position: game.worldOffset * -1,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: new Vector2(0, 0),
                scale: new Vector2(game.GraphicsDevice.Viewport.Width / 250, game.GraphicsDevice.Viewport.Height / 200),
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
