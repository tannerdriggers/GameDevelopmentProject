using System;
using System.Collections.Generic;
using System.Text;

namespace GameContentLevelsExtension
{
    public class GameMapContent
    {
        public List<Entity> entities = new List<Entity>();

        public List<GameMapLayerContent> layers = new List<GameMapLayerContent>();

        public GameMapContent(List<Entity> entities, List<GameMapLayerContent> layers)
        {
            this.entities = entities;
            this.layers = layers;
        }
    }
}
