﻿using Microsoft.Xna.Framework;
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
        private List<BackgroundModel> backgrounds;
        private Game1 game;
        public Texture2D backgroundImage;
        public Texture2D midgroundImage;

        private long totalBackgrounds = 0;

        public Background(Game1 game)
        {
            this.game = game;
            backgrounds = new List<BackgroundModel>();
        }

        public void LoadContent()
        {
            backgroundImage = game.Content.Load<Texture2D>("background");
            midgroundImage = game.Content.Load<Texture2D>("midground");

            backgrounds.Add(
                new BackgroundModel(
                    new BoundingRectangle(
                        -1 * game.GraphicsDevice.Viewport.Width - 50,
                        0,
                        game.GraphicsDevice.Viewport.Width,
                        game.GraphicsDevice.Viewport.Height
                    )
                )
            );
            backgrounds.Add(new BackgroundModel(new BoundingRectangle(-50, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height)));
        }

        public void Update(GameTime gameTime)
        {
            if (game.player.position.X - ( totalBackgrounds * game.GraphicsDevice.Viewport.Width) >= 50)
            {
                totalBackgrounds++;
                backgrounds.Add(
                    new BackgroundModel(
                        new BoundingRectangle(
                            totalBackgrounds * game.GraphicsDevice.Viewport.Width - 50,
                            0,
                            game.GraphicsDevice.Viewport.Width,
                            game.GraphicsDevice.Viewport.Height
                        )
                    )
                );

                backgrounds.RemoveAt(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //new Rectangle(
            //            (int)game.player.position.X - 100,
            //            0,
            //            game.GraphicsDevice.Viewport.Width + 1,
            //            game.GraphicsDevice.Viewport.Height
            //        )
            spriteBatch.Draw(
                texture: backgroundImage,
                position: new Vector2(game.player.position.X - 100f, 0),
                color: Color.White,
                scale: new Vector2(game.GraphicsDevice.Viewport.Width / 250, game.GraphicsDevice.Viewport.Height / 200)
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
