using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    ///  Represents a position in cartesian space.
    /// </summary>
    public class Position
    {
        public double? X { get; set; }   // Millimeters
        public double? Y { get; set; }
        public double? Z { get; set; }
        public double? Rotation { get; set; } // Degrees
    }
}