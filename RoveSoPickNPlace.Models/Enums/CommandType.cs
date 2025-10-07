namespace RoveSoPickNPlace.Models.Enums
{
    /// <summary>
    /// Enumeration representing the type of command in the system.
    /// </summary>
    public enum CommandType
    {
        Jog,       // Command to jog the machine.
        VacuumToggle, // Command to toggle the vacuum state.
        FeederAdvance, // Command to advance the feeder.
        StepGCode, // Command to step through G-code.
        Home,      // Command to home the machine.
        Stop     // Command to stop the machine.
    }
}