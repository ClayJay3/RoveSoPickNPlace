using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Enums;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a log entry for the system.
    /// </summary>
    public class LogEntry
    {
        public Guid ID { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public LogLevel? Level { get; set; }
        public string? Source { get; set; }            // GRBL, Vision, Web
        public string? Message { get; set; }

        // optional link to Job
        public Guid? JobID { get; set; }
        public Job? Job { get; set; }
    }
}