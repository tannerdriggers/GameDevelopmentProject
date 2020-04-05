using GameProject.Code.Scrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Code.Entities;

namespace GameProject.Code
{
    public class Background : Entity
    {
        private readonly Queue<BackgroundModel> backgrounds;

        public Texture2D midgroundImage;

        public Background(Game game, Texture2D texture) : base(game, texture)
        {
            backgrounds = new Queue<BackgroundModel>();
        }

        public ParallaxLayer LoadContent()
        {
            backgrounds.Enqueue(new BackgroundModel(new BoundingRectangle(-50, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height)));

            var backgroundParallax = new ParallaxLayer(Game);
            backgroundParallax.Sprites.Add(this);
            backgroundParallax.DrawOrder = 0;
            return backgroundParallax;
        }

        public new void Update(GameTime gameTime)
        {
            var offset = Game.player.PlayerScreenOffset;
            if ((backgrounds.Count - 1) * Game.GraphicsDevice.Viewport.Width > offset)
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

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture,
                position: Game.worldOffset * -1,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: new Vector2(0, 0),
                scale: new Vector2(Game.GraphicsDevice.Viewport.Width / 250, Game.GraphicsDevice.Viewport.Height / 200),
                effects: SpriteEffects.None,
                layerDepth: 0
            );

            //foreach (var background in backgrounds)
            //{
            //    spriteBatch.Draw(
            //        midgroundImage,
            //        new Rectangle(
            //            (int)background.background.X,
            //            (int)background.background.Y,
            //            (int)background.background.Width,
            //            (int)background.background.Height
            //        ),
            //        Color.White);
            //}
        }
    }
}
