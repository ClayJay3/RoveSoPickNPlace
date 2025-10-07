namespace RoveSoPickNPlace.Models.Enums
{
    /// <summary>
    /// Enumeration representing the status of a feeder in the system.
    /// </summary>
    public enum FeederStatus
    {
        Empty,      // Feeder is empty and needs to be refilled.
        Loaded,     // Feeder is loaded and ready for use.
        Jammed,    // Feeder is jammed and requires attention.
        Unknown    // Feeder status is unknown.
    }
}