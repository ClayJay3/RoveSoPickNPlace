using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoveSoPickNPlace.Models.Enums;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a camera feed connection/location.
    /// </summary>
    public class CameraFeed
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }        // Feeder/Head/Overview
        public CameraType? Type { get; set; }
        public CameraStatus? Status { get; set; }
        public string? StreamEndpoint { get; set; } // Local mjpeg/rtsp url
        public int? Fps { get; set; }
        public string? Resolution { get; set; }
        public DateTime? LastFrameAt { get; set; }
    }
}