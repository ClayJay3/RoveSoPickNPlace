namespace RoveSoPickNPlace.Models.Accounts
{
    public enum PermissionLevel
    {
        /// <summary>
        /// No permissions
        /// </summary>
        None = 0,

        /// <summary>
        /// Read-only access
        /// </summary>
        Read = 1,

        /// <summary>
        /// Read and write access
        /// </summary>
        Write = 2,

        /// <summary>
        /// Full access
        /// </summary>
        Admin = 3,

        /// <summary>
        /// Full access with additional privileges
        /// </summary>
        SuperAdmin = 4
    }
}