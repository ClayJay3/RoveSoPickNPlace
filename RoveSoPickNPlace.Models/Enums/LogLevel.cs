namespace RoveSoPickNPlace.Models.Enums
{
    /// <summary>
    /// Enumeration representing the log level in the system.
    /// </summary>
    public enum LogLevel
    {
        Debug,    // Detailed information, typically of interest only when diagnosing problems.
        Info,     // Confirmation that things are working as expected.
        Warning,  // An indication that something unexpected happened, or indicative of some problem in the near future (e.g. 'disk space low'). The software is still working as expected.
        Error,    // Due to a more serious problem, the software has not been able to perform some function.
        Critical  // A serious error, indicating that the program itself may be unable to continue running.
    }
}