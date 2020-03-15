using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Code
{
    public class GameMapContent
    {
        public List<Entity> entities = new List<Entity>();

        public List<GameMapLayerContent> layer = new List<GameMapLayerContent>();

        public GameMapContent(List<Entity> entities, List<GameMapLayerContent> layer)
        {
            this.entities = entities;
            this.layer = layer;
        }
    }
}
