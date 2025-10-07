using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents the placement of a component.
    /// </summary>
    public class ComponentPlacementRecord
    {
        public Guid ID { get; set; }
        public Guid JobID { get; set; }
        public Job? Job { get; set; }

        public Guid? ComponentDefinitionID { get; set; }
        public ComponentDefinition? ComponentDefinition { get; set; }

        public int? FeederSlot { get; set; }
        public double? TargetX { get; set; }
        public double? TargetY { get; set; }
        public double? TargetRotation { get; set; }
        public double? PlacedX { get; set; }
        public double? PlacedY { get; set; }
        public double? PlacedRotation { get; set; }
        public bool? Success { get; set; }
        public bool? CorrectionApplied { get; set; }
        public DateTime? PlacedAt { get; set; } = DateTime.UtcNow;

        // 1:1 relationship (optional)
        public InspectionResult? InspectionResult { get; set; }
    }
}