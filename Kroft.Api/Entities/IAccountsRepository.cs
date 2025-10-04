using Kroft.Models.Accounts;

namespace Kroft.Api.Entities
{
    public interface IAccountsRepository
    {
        Task<UserProfile?> AddUserProfile(UserProfile userProfile);
        Task<UserProfile?> DeleteUserProfile(System.Guid userID);
        Task<UserProfile?> UpdateUserProfile(UserProfile userProfile);
        Task<List<UserProfile>> GetAllUserProfiles();
        Task<UserProfile?> GetUserProfile(System.Guid userID);
        Task<UserProfile?> GetUserProfileByUsername(string username);
        Task<UserProfile?> GetUserProfileByEmail(string email);
        Task<UserProfile?> GetUserProfileByPhoneNumber(string phoneNumber);
        Task<PermissionLevel> GetUserPermissionLevel(System.Guid userID);
        Task<PermissionLevel> GetUserPermissionLevel(UserProfile userProfile);
        Task<bool> IsUsernameAvailable(string username);
        Task<bool> IsEmailAvailable(string email);
        Task<bool> IsPhoneNumberAvailable(string phoneNumber);
    }
}