﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code.Entities.Particles
{
    abstract class ParticleType : Entity
    {
        /// <summary>
        /// Location of the Particle Emitter
        /// </summary>
        public Vector2 EmitterLocation { get; set; }

        /// <summary>
        /// Number of Seconds the Emitter is active
        /// DEFAULT: 5 seconds
        /// </summary>
        public float? EmitterLife { get; set; }

        public float ParticleLife { get; set; } = 100f;
    }
}