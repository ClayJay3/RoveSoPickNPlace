using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Enums;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a job in the system.
    /// </summary>
    public class Job
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }                // Friendly name
        public Guid? CplFileId { get; set; }             // Reference to uploaded CPL (optional)
        public UploadedFile? CplFile { get; set; }       // navigation to uploaded file
        public JobStatus? Status { get; set; }
        public double? Progress { get; set; }            // 0.0 - 100.0
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? EstimatedDuration { get; set; }
        public string? Owner { get; set; }               // Optional user

        // Navigation collections
        public List<ComponentPlacementRecord> Placements { get; set; } = new();
        public List<BOMEntry> BomEntries { get; set; } = new();
        public List<LogEntry> LogEntries { get; set; } = new();
    }
}