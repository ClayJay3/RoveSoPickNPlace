namespace RoveSoPickNPlace.Models.Enums
{
    /// <summary>
    /// Enumeration representing the status of a machine in the system.
    /// </summary>
    public enum MachineStatus
    {
        Offline,   // Machine is not connected or powered off.
        Idle,      // Machine is powered on but not currently processing a job.
        Homing,    // Machine is performing a homing operation.
        Running,   // Machine is actively processing a job.
        Paused,    // Machine is paused and can be resumed.
        Error,      // Machine has encountered an error.
        EmergencyStop // Machine is in an emergency stop state.
    }
}