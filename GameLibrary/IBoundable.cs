using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary
{
    /// <summary>
    /// An interface defining game objects with bounds
    /// </summary>
    public interface IBoundable
    {
        BoundingRectangle Bounds { get; }
    }
}
