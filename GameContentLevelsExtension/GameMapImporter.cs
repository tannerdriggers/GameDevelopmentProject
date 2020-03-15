using System;
using System.Collections.Generic;
using System.Text.Json;

//using Newtonsoft.Json;
using Microsoft.Xna.Framework.Content.Pipeline;

//using TInput = GameContentLevelsExtension.GameMapContent;
using System.IO;

namespace GameContentLevelsExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentImporter(".json", DisplayName = "JSON Importer - Tiled", DefaultProcessor = "GameMapProcessor")]
    public class GameMapImporter : ContentImporter<string>
    {
        /// <summary>
        /// Reads in the json file and maps it to a GameMapContent object
        /// </summary>
        /// <param name="filename">Name of the json level file</param>
        /// <param name="context">The pipeline context</param>
        /// <returns>GameMapContent object containing the level</returns>
        public override string Import(string filename, ContentImporterContext context)
        {
            using (StreamReader r = new StreamReader(filename))
            {
                return r.ReadToEnd();
            }
        }
    }
}
