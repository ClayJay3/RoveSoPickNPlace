using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoveSoPickNPlace.Models.Entities
{
    /// <summary>
    /// Represents an uploaded file in the system.
    /// </summary>
    public class UploadedFile
    {
        public Guid ID { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long? SizeBytes { get; set; }
        public DateTime? UploadedAt { get; set; }
        public string? StoragePath { get; set; }         // path on Pi or blob url
        public bool? Parsed { get; set; }
        public string? ParsingErrors { get; set; }
        public string? BoardName { get; set; }           // optional metadata

        // optional reverse nav: jobs that reference this file (in practice likely 0..1)
        public List<Job>? JobsReferencing { get; set; }
    }
}