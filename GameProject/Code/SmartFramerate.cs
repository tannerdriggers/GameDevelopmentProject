using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Code
{
    class SmartFramerate
    {
        double currentFrametimes;
        readonly double weight;
        readonly int numerator;


        public double Framerate => numerator / currentFrametimes;

        public SmartFramerate(int oldFrameWeight)
        {
            numerator = oldFrameWeight;
            weight = oldFrameWeight / (oldFrameWeight - 1d);
        }

        public void Update(double timeSinceLastFrame)
        {
            currentFrametimes /= weight;
            currentFrametimes += timeSinceLastFrame;
        }
    }
}
