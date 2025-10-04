using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PointFilter
{
    // These properties define a specific point and radius for filtering.
    public double? Easting { get; set; }
    public double? Northing { get; set; }
    public double? Radius { get; set; }

    // This property is a nullable reference type, allowing it to be 'null'.
    public string? Classification { get; set; }

    // This is a nested generic struct to represent a min/max range.
    public struct Range<T> where T : struct
    {
        public T Min { get; set; }
        public T Max { get; set; }
    }
    
    // These properties are nullable value types, using '?' to represent
    // an optional range that may or may not be set.
    public Range<double>? NormalX { get; set; }
    public Range<double>? NormalY { get; set; }
    public Range<double>? NormalZ { get; set; }
    public Range<double>? Slope { get; set; }
    public Range<double>? Roughness { get; set; }
    public Range<double>? Curvature { get; set; }
    public Range<double>? TraversalScore { get; set; }
}