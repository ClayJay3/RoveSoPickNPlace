using Kroft.Models.Accounts;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Kroft.Web.Core.Services
{
    public class AccountsService
    {
        // Injected services.
        private readonly HttpClient _HttpClient;
        private readonly ProtectedLocalStorage _ProtectedLocalStorage;
        private readonly ProtectedSessionStorage _ProtectedSessionStorage;
        // Declare member variables.
        public UserProfile? CurrentUserProfile { get; private set; }
        public string? CurrentToken { get; private set; }
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">Implicitly passed in, used to talk to the kroft API.</param>
        /// <param name="protectedLocalStorage">Implicitly passed in, used to store data in the browser's local storage.</param>
        /// <param name="protectedSessionStorage">Implicitly passed in, used to store data in the browser's session storage.</param>
        public AccountsService(HttpClient httpClient, ProtectedLocalStorage protectedLocalStorage, ProtectedSessionStorage protectedSessionStorage)
        {
            // Disable SSL verification for development
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _HttpClient = new HttpClient(handler)
            {
                BaseAddress = httpClient.BaseAddress
            };

            _ProtectedLocalStorage = protectedLocalStorage;
            _ProtectedSessionStorage = protectedSessionStorage;
        }

        /// <summary>
        /// Check the persistent storage for authentication data and restore it.
        /// </summary>
        public async Task CheckAndRestoreAuthentication()
        {
            // Check the persistent storage for the AuthData first.
            var authData = await _ProtectedLocalStorage.GetAsync<AuthData>("authData");
            if (authData.Success && authData.Value is not null)
            {
                // Add the current token to the http bearer client.
                _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authData.Value.Token);

                // Update the current user profile, token and authentication status.
                CurrentUserProfile = await GetUserProfileByUsername(authData.Value.Username);
                CurrentToken = authData.Value.Token;
                IsAuthenticated = true;
            }
            // Check the session storage for the AuthData next.
            else
            {
                var sessionAuthData = await _ProtectedSessionStorage.GetAsync<AuthData>("authData");
                if (sessionAuthData.Success && sessionAuthData.Value is not null)
                {
                    // Add the current token to the http bearer client.
                    _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionAuthData.Value.Token);

                    // Update the current user profile, token and authentication status.
                    CurrentUserProfile = await GetUserProfileByUsername(sessionAuthData.Value.Username);
                    CurrentToken = sessionAuthData.Value.Token;
                    IsAuthenticated = true;
                }
            }
        }

        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="loginCreds">The credentials to authenticate with.</param>
        /// <returns>The authentication token, null if not authenticated.</returns>
        public async Task<string?> Authenticate(LoginCreds loginCreds)
        {
            HttpResponseMessage response = await _HttpClient.PostAsJsonAsync("Accounts/authenticate", loginCreds);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("token", out JsonElement tokenElement))
                    {
                        // Get the token from the response.
                        string token = tokenElement.GetString() ?? string.Empty;
                        // Add the current token to the http bearer client.
                        _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        // Update the current user profile, token and authentication status.
                        CurrentUserProfile = await GetUserProfileByUsername(loginCreds.Username);
                        CurrentToken = token;
                        IsAuthenticated = true;

                        return token;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Refresh the current authentication token.
        /// </summary>
        /// <returns>The refreshed token, null if not successful.</returns>
        public async Task<string?> RefreshToken()
        {
            var response = await _HttpClient.PostAsJsonAsync("Accounts/refresh-token", CurrentToken);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("token", out JsonElement tokenElement))
                    {
                        // Update the current token.
                        CurrentToken = tokenElement.GetString() ?? string.Empty;
                        // Update the authentication header.
                        _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentToken);
                        return CurrentToken;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Minimal example for calling the "refresh-token" API.
        /// </summary>
        /// <param name="oldToken">The old token to refresh.</param>
        /// <returns>The new token, null if not successful.</returns>
        public async Task<string?> RefreshTokenAsync(string oldToken)
        {
            var response = await _HttpClient.PostAsJsonAsync("api/Accounts/refresh-token", oldToken);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return result?.Token;
        }

        /// <summary>
        /// Logout the current user.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        public bool Logout()
        {
            // Clear the current user profile and token.
            CurrentUserProfile = null;
            CurrentToken = null;
            IsAuthenticated = false;

            // Clear the authentication header.
            _HttpClient.DefaultRequestHeaders.Authorization = null;

            return true;
        }

        /// <summary>
        /// Add a user profile to the database.
        /// </summary>
        /// <param name="userProfile">The user profile to add.</param>
        /// <returns>The user profile as it now is in the database.</returns>
        public async Task<UserProfile?> AddUserProfile(UserProfile userProfile)
        {
            HttpResponseMessage response = await _HttpClient.PutAsJsonAsync("Accounts", userProfile);
            if (response.IsSuccessStatusCode)
            {
                UserProfile? dbUserProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return dbUserProfile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Delete a user profile from the database.
        /// </summary>
        /// <param name="userID">The ID of the user profile to delete.</param>
        /// <returns>The user profile as it now is in the database.</returns>
        public async Task<UserProfile?> DeleteUserProfile(System.Guid userID)
        {
            HttpResponseMessage response = await _HttpClient.DeleteAsync($"Accounts/{userID}");
            if (response.IsSuccessStatusCode)
            {
                UserProfile? dbUserProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return dbUserProfile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update a user profile in the database.
        /// </summary>
        /// <param name="userProfile">The user profile to update.</param>
        /// <returns>The updated user profile as it now is in the database.</returns>
        public async Task<UserProfile?> UpdateUserProfile(UserProfile userProfile)
        {
            HttpResponseMessage response = await _HttpClient.PostAsJsonAsync("Accounts", userProfile);
            if (response.IsSuccessStatusCode)
            {
                UserProfile? dbUserProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return dbUserProfile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get all user profiles in the DB.
        /// </summary>
        /// <returns>A list of UserProfile objects.</returns>
        public async Task<List<UserProfile>?> GetUserProfiles()
        {
            List<UserProfile>? dbUserProfiles = await _HttpClient.GetFromJsonAsync<List<UserProfile>>("Accounts");
            return dbUserProfiles;
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="userID">The ID of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfile(System.Guid userID)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/{userID}");
            if (response.IsSuccessStatusCode)
            {
                UserProfile? dbUserProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return dbUserProfile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="email">The email of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfileByEmail(string email)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/email/{email}");
            if (response.IsSuccessStatusCode)
            {
                UserProfile? dbUserProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return dbUserProfile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfileByPhoneNumber(string phoneNumber)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/phone/{phoneNumber}");
            if (response.IsSuccessStatusCode)
            {
                UserProfile? dbUserProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return dbUserProfile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a user profile from the DB.
        /// </summary>
        /// <param name="username">The username of the user profile to return.</param>
        /// <returns>A UserProfile object, null if not found.</returns>
        public async Task<UserProfile?> GetUserProfileByUsername(string username)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/username/{username}");
            if (response.IsSuccessStatusCode)
            {
                UserProfile? dbUserProfile = await response.Content.ReadFromJsonAsync<UserProfile>();
                return dbUserProfile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the permission level of a user.
        /// </summary>
        /// <param name="userID">The ID of the user to get the permission level for.</param>
        /// <returns>The permission level of the user.</returns>
        public async Task<PermissionLevel> GetUserPermissionLevel(System.Guid userID)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/permission/{userID}");
            if (response.IsSuccessStatusCode)
            {
                PermissionLevel permissionLevel = await response.Content.ReadFromJsonAsync<PermissionLevel>();
                return permissionLevel;
            }
            else
            {
                return PermissionLevel.None;
            }
        }

        /// <summary>
        /// Get the permission level of a user.
        /// </summary>
        /// <param name="userProfile">The user profile to get the permission level for.</param>
        /// <returns>The permission level of the user.</returns>
        public async Task<PermissionLevel> GetUserPermissionLevel(UserProfile userProfile)
        {
            HttpResponseMessage response = await _HttpClient.PostAsJsonAsync("Accounts/permissionLevel", userProfile);
            if (response.IsSuccessStatusCode)
            {
                PermissionLevel permissionLevel = await response.Content.ReadFromJsonAsync<PermissionLevel>();
                return permissionLevel;
            }
            else
            {
                return PermissionLevel.None;
            }
        }

        /// <summary>
        /// Check if a username is available.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if available, false if not.</returns>
        public async Task<bool> IsUsernameAvailable(string username)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/username/{username}/available");
            if (response.IsSuccessStatusCode)
            {
                bool isAvailable = await response.Content.ReadFromJsonAsync<bool>();
                return isAvailable;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if an email is available.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if available, false if not.</returns>
        public async Task<bool> IsEmailAvailable(string email)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/email/{email}/available");
            if (response.IsSuccessStatusCode)
            {
                bool isAvailable = await response.Content.ReadFromJsonAsync<bool>();
                return isAvailable;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if a phone number is available.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <returns>True if available, false if not.</returns>
        public async Task<bool> IsPhoneNumberAvailable(string phoneNumber)
        {
            HttpResponseMessage response = await _HttpClient.GetAsync($"Accounts/phone/{phoneNumber}/available");
            if (response.IsSuccessStatusCode)
            {
                bool isAvailable = await response.Content.ReadFromJsonAsync<bool>();
                return isAvailable;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Simple record for reading JSON.
    /// </summary>
    internal record TokenResponse(string Token);
}