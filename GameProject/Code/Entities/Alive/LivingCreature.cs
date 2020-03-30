using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Alive
{
    abstract class LivingCreature : Entity
    {
        /// <summary>
        /// Whether or not a Living Creature is Alive
        /// </summary>
        public bool Alive { get; set; }
    }
}
