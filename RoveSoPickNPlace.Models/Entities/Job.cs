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
    public Guid? CplFileId { get; set; }             // Reference to uploaded CPL
    public JobStatus? Status { get; set; }
    public double? Progress { get; set; }            // 0.0 - 100.0
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public TimeSpan? EstimatedDuration { get; set; }
    public string? Owner { get; set; }               // Optional user
    public List<ComponentPlacementRecord>? Placements { get; set; }
    public byte[]? RowVersion { get; set; }          // Concurrency token
    }
}