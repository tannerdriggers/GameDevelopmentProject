using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Alive
{
    public abstract class EnemyType : LivingCreature
    {
        public EnumEnemyType enemyType { get; set; }

        public EnemyType(Game game, Texture2D texture) : base(game, texture)
        {
        }
    }
}
