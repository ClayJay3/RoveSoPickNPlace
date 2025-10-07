using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Enums;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a command sent to manually control the machine.
    /// </summary>
    public class ManualControlCommand
    {
        public Guid ID { get; set; }
        public CommandType? Type { get; set; }
        public string? ParametersJson { get; set; }
        public string? IssuedBy { get; set; }
        public DateTime? IssuedAt { get; set; }
        public bool? Executed { get; set; }
        public DateTime? ExecutedAt { get; set; }
    }
}