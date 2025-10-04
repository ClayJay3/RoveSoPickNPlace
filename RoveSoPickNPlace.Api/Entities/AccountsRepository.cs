using RoveSoPickNPlace.Models.Accounts;
using Microsoft.EntityFrameworkCore;

namespace RoveSoPickNPlace.Api.Entities
{
    public class AccountsRepository : IAccountsRepository
    {
        // Declare member variables.
        private readonly RoveSoPickNPlaceDatabase _RoveSoPickNPlaceDatabase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db">Implicitly passed in.</param>
        public AccountsRepository(RoveSoPickNPlaceDatabase db)
        {
            _RoveSoPickNPlaceDatabase = db;
        }

        public async Task<UserProfile?> AddUserProfile(UserProfile userProfile)
        {
            // Make sure the ID is null.
            userProfile.ID = null;
            // Add new row to database table.
            var result = await _RoveSoPickNPlaceDatabase.UserProfiles.AddAsync(userProfile);
            await _RoveSoPickNPlaceDatabase.SaveChangesAsync();
            // Return the inserted value.
            return result.Entity;
        }

        public async Task<UserProfile?> DeleteUserProfile(System.Guid userID)
        {
            // Find the first user profile with the same ID.
            UserProfile? result = await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.ID == userID);
            // Check if it was found.
            if (result is not null)
            {
                // Remove the row from the database.
                _RoveSoPickNPlaceDatabase.UserProfiles.Remove(result);
                await _RoveSoPickNPlaceDatabase.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// Update a user profile in the database.
        /// </summary>
        /// <param name="userProfile">The user profile to update.</param>
        /// <returns>The updated user profile as it now is in the database.</returns>
        public async Task<UserProfile?> UpdateUserProfile(UserProfile userProfile)
        {
            // Find the first user profile with the same ID.
            UserProfile? result = await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.ID == userProfile.ID);
            // Check if it was found.
            if (result is not null)
            {
                // Update the row in the database.
                _RoveSoPickNPlaceDatabase.UserProfiles.Update(userProfile);
                await _RoveSoPickNPlaceDatabase.SaveChangesAsync();
            }
            return result;
        }

        /// <summary>
        /// Get all user profiles in the DB.
        /// </summary>
        /// <returns>A list of UserProfile objects.</returns>
        public async Task<List<UserProfile>> GetAllUserProfiles()
        {
            return await _RoveSoPickNPlaceDatabase.UserProfiles.ToListAsync();
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="userID">The ID of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfile(System.Guid userID)
        {
            return await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.ID == userID);
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="email">The email of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfileByEmail(string email)
        {
            return await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfileByPhoneNumber(string phoneNumber)
        {
            return await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="username">The username of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfileByUsername(string username)
        {
            return await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.Username == username);
        }

        /// <summary>
        /// Get the permission level of a user.
        /// </summary>
        /// <param name="userID">The ID of the user to get the permission level for.</param>
        /// <returns>The permission level of the user.</returns>
        public async Task<PermissionLevel> GetUserPermissionLevel(System.Guid userID)
        {
            // Get the user profile.
            UserProfile? result = await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.ID == userID);
            // Check if it was found.
            if (result is not null)
            {
                return result.PermissionLevel ?? PermissionLevel.Read;
            }
            return PermissionLevel.None;
        }

        /// <summary>
        /// Get the permission level of a user.
        /// </summary>
        /// <param name="userProfile">The user profile to get the permission level for.</param>
        /// <returns>The permission level of the user.</returns>
        public async Task<PermissionLevel> GetUserPermissionLevel(UserProfile userProfile)
        {
            // Get the user profile.
            UserProfile? result = await _RoveSoPickNPlaceDatabase.UserProfiles.FirstOrDefaultAsync(x => x.ID == userProfile.ID);
            // Check if it was found.
            if (result is not null)
            {
                return result.PermissionLevel ?? PermissionLevel.Read;
            }
            return PermissionLevel.None;
        }

        /// <summary>
        /// Check if a username is available.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the username is available, false otherwise.</returns>
        public async Task<bool> IsUsernameAvailable(string username)
        {
            // Check if the username is already taken.
            return await _RoveSoPickNPlaceDatabase.UserProfiles.AnyAsync(x => x.Username == username) == false;
        }

        /// <summary>
        /// Check if an email is available.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is available, false otherwise.</returns>
        public async Task<bool> IsEmailAvailable(string email)
        {
            // Check if the email is already taken.
            return await _RoveSoPickNPlaceDatabase.UserProfiles.AnyAsync(x => x.Email == email) == false;
        }

        /// <summary>
        /// Check if a phone number is available.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <returns>True if the phone number is available, false otherwise.</returns>
        public async Task<bool> IsPhoneNumberAvailable(string phoneNumber)
        {
            // Check if the phone number is already taken.
            return await _RoveSoPickNPlaceDatabase.UserProfiles.AnyAsync(x => x.PhoneNumber == phoneNumber) == false;
        }

    }
}