using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents a camera calibration externally obtained/calculated.
    /// </summary>
    public class VisionCalibration
    {
        public Guid ID { get; set; }
        public Guid CameraID { get; set; }
        public string? IntrinsicsJson { get; set; }   // fx,fy,cx,cy
        public string? DistortionJson { get; set; }   // k1,k2,p1,p2,...
        public DateTime? CalibratedAt { get; set; }
    }
}