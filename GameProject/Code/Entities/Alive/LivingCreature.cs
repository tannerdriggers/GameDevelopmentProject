using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Alive
{
    public abstract class LivingCreature : Entity
    {
        /// <summary>
        /// Whether or not a Living Creature is Alive
        /// </summary>
        public bool Alive { get; set; }

        public LivingCreature(Game game, Texture2D texture) : base(game, texture)
        {
            Game = game;
            Texture = texture;
        }
    }
}
