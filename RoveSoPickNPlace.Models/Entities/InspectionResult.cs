using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    ///  Represents a result from computer vision inspection.
    /// </summary>
    public class InspectionResult
    {
        public Guid ID { get; set; }
        public bool? IsCorrect { get; set; }
        public double? OffsetX { get; set; }
        public double? OffsetY { get; set; }
        public double? RotationError { get; set; }
        public string? ImagePath { get; set; }      // For debugging / audit
        public DateTime? Timestamp { get; set; }
    }
}