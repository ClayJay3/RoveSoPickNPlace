using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a single line item in the pick and place job's Bill of Materials.
    /// </summary>
    public class BOMEntry
    {
        public Guid ID { get; set; }
        public Guid JobID { get; set; }
        public Job? Job { get; set; }
        public string? Designator { get; set; }       // e.g., R1, C3
        public string? PartNumber { get; set; }
        public int? Quantity { get; set; }
        public int? AssignedFeederSlot { get; set; }
    }
}