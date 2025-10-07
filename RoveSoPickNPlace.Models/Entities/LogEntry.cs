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
        public DateTime? Timestamp { get; set; }
        public LogLevel? Level { get; set; }
        public string? Source { get; set; }            // GRBL, Vision, Web
        public string? Message { get; set; }
        public Guid? JobId { get; set; }
    }
}