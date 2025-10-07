using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a component definition in the system.
    /// </summary>
    public class ComponentDefinition
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }                // user friendly / part number
        public string? PackageType { get; set; }         // e.g., 0603, SOIC-8
        public double? LengthMm { get; set; }
        public double? WidthMm { get; set; }
        public double? HeightMm { get; set; }
        public double? RotationOffsetDegrees { get; set; }
        public double? PickupHeightOffsetMm { get; set; }
        public double? TapePitchMm { get; set; }
        public string? TapeOrientationCode { get; set; } // M1, C1 etc.
        public bool VisionAlignmentRequired { get; set; }
        public string? Notes { get; set; }
    }
}