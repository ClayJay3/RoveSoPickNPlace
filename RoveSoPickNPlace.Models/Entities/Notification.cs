using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Enums;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a system notif.
    /// </summary>
    public class Notification
    {
        public Guid ID { get; set; }
        public LogLevel? Level { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Resolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}