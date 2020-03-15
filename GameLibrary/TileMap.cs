using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary
{
    public class TileMap
    {
        public string json;

        /// <summary>
        /// Constructs a new instance of a Tilemap
        /// </summary>
        public TileMap(string json)
        {
            this.json = json;
        }

        ///// <summary>
        ///// Draws the specified layer of the tilemap
        ///// </summary>
        ///// <param name="spriteBatch">The SpriteBatch to draw with</param>
        ///// <param name="layerIndex">The index of the layer to use</param>
        //public void DrawLayer(SpriteBatch spriteBatch, int layerIndex)
        //{
        //    var layer = layers[layerIndex];
        //    for (int y = 0; y < layer.Height; y++)
        //    {
        //        for (int x = 0; x < layer.Width; x++)
        //        {
        //            int tileIndex = layer.Data[x][y];
        //            if (tileIndex != 0 && tileIndex < tiles.Length)
        //            {
        //                Vector2 position = new Vector2(x * layer.Tilesize, y * layer.Tilesize);
        //                tiles[tileIndex].Draw(spriteBatch, position, Color.White);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Draws the entire Tilemap
        ///// </summary>
        ///// <param name="spriteBatch">The SpriteBatch to draw with</param>
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach (var layer in layers)
        //    {
        //        for (int y = 0; y < layer.Height; y++)
        //        {
        //            for (int x = 0; x < layer.Width; x++)
        //            {
        //                int tileIndex = layer.Data[x][y];
        //                if (tileIndex != 0 && tileIndex < tiles.Length)
        //                {
        //                    Vector2 position = new Vector2(x * layer.Tilesize, y * layer.Tilesize);
        //                    tiles[tileIndex].Draw(spriteBatch, position, Color.White);
        //                }
        //            }
        //        }
        //    }

            // TODO: Draw the entities
        //}
    }
}
