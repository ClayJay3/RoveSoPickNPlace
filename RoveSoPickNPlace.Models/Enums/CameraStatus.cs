namespace RoveSoPickNPlace.Models.Enums
{
    /// <summary>
    /// Enumeration representing the status of a camera in the system.
    /// </summary>
    public enum CameraStatus
    {
        Disconnected, // Camera is not connected.
        Connected,    // Camera is connected but not streaming.
        Streaming,    // Camera is actively streaming video.
        Error         // Camera has encountered an error.
    }
}