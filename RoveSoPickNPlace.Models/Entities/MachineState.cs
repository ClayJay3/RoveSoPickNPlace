using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Enums;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents the current state of the machine.
    /// </summary>
    public class MachineState
    {
        public Guid ID { get; set; }
        public MachineStatus? Status { get; set; }
        public bool? GrblConnected { get; set; }
        public bool? RaspberryPiConnected { get; set; }
        public Position? CurrentPosition { get; set; }
        public double? CpuTemperatureC { get; set; }
        public double? CpuUsagePercent { get; set; }
        public double? MemoryUsagePercent { get; set; }
        public Guid? ActiveJobId { get; set; }
        public DateTime? LastHeartbeat { get; set; }
    }
}