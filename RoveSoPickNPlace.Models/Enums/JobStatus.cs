namespace RoveSoPickNPlace.Models.Enums
{
    /// <summary>
    /// Enumeration representing the status of a job in the system.
    /// </summary>
    public enum JobStatus
    {
        Pending,  // Job is created but not yet started.
        Running,  // Job is currently in progress.
        Paused,   // Job is paused and can be resumed.
        Completed,// Job has finished successfully.
        Failed,   // Job has failed.
        Canceled  // Job has been canceled.
    }
}