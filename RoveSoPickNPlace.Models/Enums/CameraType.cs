namespace RoveSoPickNPlace.Models.Enums
{
    /// <summary>
    /// Enumeration representing the type of camera in the system.
    /// </summary>
    public enum CameraType
    {
        Feeder,  // Camera used for feeder alignment and verification.
        Vision,   // Camera used for vision processing and part recognition.
        Overview // Camera providing an overview of the workspace.
    }
}