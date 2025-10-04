using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("LiDARPoints")]
public class LiDARPoint
{
    [Key]
    public int ID { get; set; }

    [Required]
    public double? Easting { get; set; }

    [Required]
    public double? Northing { get; set; }

    public double? Altitude { get; set; }

    [MaxLength(8)]
    public string? Zone { get; set; }

    [MaxLength(64)]
    public string? Classification { get; set; }

    // Precomputed metrics.
    public double? NormalX { get; set; }
    public double? NormalY { get; set; }
    public double? NormalZ { get; set; }

    public double? Slope { get; set; }       // degrees
    public double? Rough { get; set; }       // RMS error
    public double? Curvature { get; set; }   // curvature estimate
    public double? TravScore { get; set; }   // traversal score 0..1
}