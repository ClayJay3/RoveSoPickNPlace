using RoveSoPickNPlace.Api.Entities;
using RoveSoPickNPlace.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace RoveSoPickNPlace.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // Declare member variables.
        private readonly IAccountsRepository _AccountsRepository;
        private readonly IConfiguration _Configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountsController(IAccountsRepository AccountsRepository, IConfiguration Configuration)
        {
            _AccountsRepository = AccountsRepository;
            _Configuration = Configuration;
            _AccountsRepository = AccountsRepository;
        }

        /// <summary>
        /// IN-Code API Endpoint for authenticating a user.
        /// </summary>
        /// <param name="loginInfo">The login credentials.</param>
        /// <returns>The API response object.</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginCreds loginInfo)
        {
            var userProfile = await _AccountsRepository.GetUserProfileByUsername(loginInfo.Username);
            if (userProfile != null && AccountUtils.VerifyPassword(loginInfo.Password, userProfile.HashedPassword, userProfile.Salt, userProfile.Argon2DegreeOfParallelism, userProfile.Argon2Iterations, userProfile.Argon2MemorySize))
            {
                var token = GenerateJwtToken(userProfile);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Generates a JWT token for the given user profile.
        /// </summary>
        /// <param name="userProfile">The user profile to generate the token for.</param>
        /// <returns>The generated JWT token.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the username or ID is null.</exception>
        private string GenerateJwtToken(UserProfile userProfile)
        {
            // Make sure the username and ID are not null.
            if (userProfile.Username is null || userProfile.ID.ToString() is null)
            {
                throw new ArgumentNullException("Username or ID is null");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentNullException("Jwt:Key", "JWT key is not configured.");
            }
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userProfile.Username),
                    new Claim(ClaimTypes.NameIdentifier, userProfile.ID.ToString() ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _Configuration["Jwt:Issuer"],
                Audience = _Configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// IN-Code API Endpoint for adding a user profile to the DB.
        /// </summary>
        /// <param name="userProfile">The user profile object.</param>
        /// <returns>The API response object.</returns>
        [HttpPut]
        public async Task<IActionResult> AddUserProfile(UserProfile userProfile)
        {
            UserProfile? dbUserProfile = await _AccountsRepository.AddUserProfile(userProfile);
            if (dbUserProfile is not null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// /// IN-Code API Endpoint for deleting a user profile from the DB.
        /// </summary>
        /// <param name="userID">The ID of the user profile to delete.</param>
        /// <returns>The API response object.</returns>
        [HttpDelete("{userID}")]
        public async Task<IActionResult> DeleteUserProfile(System.Guid userID)
        {
            UserProfile? dbUserProfile = await _AccountsRepository.DeleteUserProfile(userID);
            if (dbUserProfile is not null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for updating a user profile in the DB.
        /// </summary>
        /// <param name="userProfile">The user profile object.</param>
        /// <returns>The API response object.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile(UserProfile userProfile)
        {
            UserProfile? dbUserProfile = await _AccountsRepository.UpdateUserProfile(userProfile);
            if (dbUserProfile is not null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for getting all user profiles from the DB.
        /// </summary>
        /// <returns>The API response object.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles()
        {
            List<UserProfile> userProfiles = await _AccountsRepository.GetAllUserProfiles();
            return Ok(userProfiles);
        }

        /// <summary>
        /// IN-Code API Endpoint for getting a user profile by ID from the DB.
        /// </summary>
        /// <param name="userID">The ID of the user profile to get.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserProfile(System.Guid userID)
        {
            UserProfile? dbUserProfile = await _AccountsRepository.GetUserProfile(userID);
            if (dbUserProfile is not null)
            {
                return Ok(dbUserProfile);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for getting a user profile by email from the DB.
        /// </summary>
        /// <param name="email">The email of the user profile to get.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserProfileByEmail(string email)
        {
            UserProfile? dbUserProfile = await _AccountsRepository.GetUserProfileByEmail(email);
            if (dbUserProfile is not null)
            {
                return Ok(dbUserProfile);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for getting a user profile by phone number from the DB.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the user profile to get.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("phone/{phoneNumber}")]
        public async Task<IActionResult> GetUserProfileByPhoneNumber(string phoneNumber)
        {
            UserProfile? dbUserProfile = await _AccountsRepository.GetUserProfileByPhoneNumber(phoneNumber);
            if (dbUserProfile is not null)
            {
                return Ok(dbUserProfile);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for getting a user profile by username from the DB.
        /// </summary>
        /// <param name="username">The username of the user profile to get.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserProfileByUsername(string username)
        {
            UserProfile? dbUserProfile = await _AccountsRepository.GetUserProfileByUsername(username);
            if (dbUserProfile is not null)
            {
                return Ok(dbUserProfile);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// IN-Code API Endpoint for getting the permission level of a user by ID.
        /// </summary>
        /// <param name="userID">The ID of the user to get the permission level for.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("permissionLevel/{userID}")]
        public async Task<IActionResult> GetUserPermissionLevel(System.Guid userID)
        {
            PermissionLevel permissionLevel = await _AccountsRepository.GetUserPermissionLevel(userID);
            return Ok(permissionLevel);
        }

        /// <summary>
        /// IN-Code API Endpoint for getting the permission level of a user by user profile.
        /// </summary>
        /// <param name="userProfile">The user profile to get the permission level for.</param>
        /// <returns>The API response object.</returns>
        [HttpPost("permissionLevel")]
        public async Task<IActionResult> GetUserPermissionLevel(UserProfile userProfile)
        {
            PermissionLevel permissionLevel = await _AccountsRepository.GetUserPermissionLevel(userProfile);
            return Ok(permissionLevel);
        }

        /// <summary>
        /// IN-Code API Endpoint for checking if a username is available.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("isUsernameAvailable/{username}")]
        public async Task<IActionResult> IsUsernameAvailable(string username)
        {
            bool isAvailable = await _AccountsRepository.IsUsernameAvailable(username);
            return Ok(isAvailable);
        }

        /// <summary>
        /// IN-Code API Endpoint for checking if an email is available.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("isEmailAvailable/{email}")]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            bool isAvailable = await _AccountsRepository.IsEmailAvailable(email);
            return Ok(isAvailable);
        }

        /// <summary>
        /// IN-Code API Endpoint for checking if a phone number is available.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <returns>The API response object.</returns>
        [HttpGet("isPhoneNumberAvailable/{phoneNumber}")]
        public async Task<IActionResult> IsPhoneNumberAvailable(string phoneNumber)
        {
            bool isAvailable = await _AccountsRepository.IsPhoneNumberAvailable(phoneNumber);
            return Ok(isAvailable);
        }
    }
}