using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoveSoPickNPlace.Models.Accounts
{
    [Table("UserProfiles")]
    public class UserProfile
    {
        [Key]
        public Guid? ID { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? HashedPassword { get; set; }
        [Required]
        public string? Salt { get; set; }
        [Required]
        public int? Argon2DegreeOfParallelism { get; set; }
        [Required]
        public int? Argon2Iterations { get; set; }
        [Required]
        public int? Argon2MemorySize { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public PermissionLevel? PermissionLevel { get; set; }
    }
}