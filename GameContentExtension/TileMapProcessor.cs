using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

using TInput = GameContentExtension.TileMapContent;
using TOutput = GameContentExtension.TileMapContent;

namespace GameContentExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to tileset data 
    /// </summary>
    [ContentProcessor(DisplayName = "TileMap Processor - Tiled")]
    public class TilemapProcessor : ContentProcessor<TInput, TOutput>
    {
        /// <summary>
        /// Processes the raw .tsx XML and creates a TilesetContent
        /// for use in an XNA framework game
        /// </summary>
        /// <param name="input">The XML string</param>
        /// <param name="context">The pipeline context</param>
        /// <returns>A TilesetContent instance corresponding to the tsx input</returns>
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            // Process the Tilesets
            for (int i = 0; i < input.TileSets.Count; i++)
            {
                var tileset = input.TileSets[i];
                ExternalReference<TileSetContent> externalRef = new ExternalReference<TileSetContent>(tileset.Source);
                tileset.Reference = context.BuildAsset<TileSetContent, TileSetContent>(externalRef, "TileSetProcessor");
            }

            foreach (TileMapLayerContent layer in input.Layers)
            {
                List<uint> dataIds = new List<uint>();
                foreach (var id in layer.DataString.Split(','))
                {
                    dataIds.Add(uint.Parse(id));
                };
                layer.Data = dataIds.ToArray();
            }

            // The tileset has been processed
            return input;
        }
    }
}
