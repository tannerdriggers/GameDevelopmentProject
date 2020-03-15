using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = GameLibrary.TileMap;

namespace GameLibrary
{
    public class TileMapReader : ContentTypeReader<string>
    {
        protected override string Read(ContentReader input, string existingInstance)
        {
            // Read in the content properties in the exact same 
            // order they were written in the corresponding writer
            
            // Read the layer count and create collections to hold layers
            //var layerCount = input.ReadInt32();
            //var layers = new TileMapLayer[layerCount];
            //var layersByName = new Dictionary<string, TileMapLayer>();

            // Read the layer data
            //for (var i = 0; i < layerCount; i++)
            //{
                //var layerId = input.ReadUInt32();
                //var layerName = input.ReadString();
                //var tileCount = input.ReadUInt32();
                //var layerData = new int[tileCount];
                //for (uint j = 0; j < tileCount; j++)
                //{
                //    layerData[j] = input.ReadInt32();
                //}
                //layers[i] = new TileMapLayer(layerData);
                //layersById[layerId] = layers[i];
                //layersByName[layerName] = layers[i];
            //}

            // Read the tileset data
            //var tiles = new List<Tile>() { new Tile() };
            //var tilesetCount = input.ReadInt32();
            //for (var i = 0; i < tilesetCount; i++)
            //{
            //    var nextGUID = input.ReadInt32();
            //    if (nextGUID != tiles.Count) throw new Exception("Tileset GUIDs do not match");
            //    var tileset = input.ReadExternalReference<TileSet>();
            //    // Create the tiles from the tileset 
            //    for (int j = 0; j < tileset.Count; j++)
            //    {
            //        tiles.Add(tileset[j]);
            //    }
            //}

            // Construct and return the tilemap
            return input.ReadString();
        }
    }
}
