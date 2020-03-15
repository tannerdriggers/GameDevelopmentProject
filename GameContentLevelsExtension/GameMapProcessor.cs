using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = GameContentLevelsExtension.GameMapContent;
using TOutput = GameContentLevelsExtension.GameMapContent;

namespace GameContentLevelsExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to tileset data 
    /// </summary>
    [ContentProcessor(DisplayName = "GameMapProcessor")]
    public class GameMapProcessor : ContentProcessor<string, string>
    {
        /// <summary>
        /// returns the input since it is already processed by the Importer
        /// </summary>
        /// <param name="input">GameMapContent object</param>
        /// <param name="context">The pipeline context</param>
        /// <returns>A GameMapContent object</returns>
        public override string Process(string input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
