using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Enums;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a feeder in the system.
    /// </summary>
    public class Feeder
    {
        public Guid ID { get; set; }
        public int? SlotNumber { get; set; }             // 1..N

        // component definition
        public Guid ComponentDefinitionId { get; set; }
        public ComponentDefinition? ComponentDefinition { get; set; }

        public FeederStatus? Status { get; set; }
        public bool? IsLoaded { get; set; }
        public int? RemainingCount { get; set; }         // Optional
        public DateTime? LastLoadedAt { get; set; }

        // owned position
        public Position? PickupPosition { get; set; }
    }
}