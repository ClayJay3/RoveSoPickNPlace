using System;
using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace RoveSoPickNPlace.Models.Accounts
{
    public static class AccountUtils
    {
        /// <summary>
        /// Generates a cryptographically secure random salt.
        /// </summary>
        /// <param name="size">The size of the salt in bytes. Default is 16 bytes.</param>
        /// <returns>A Base64-encoded salt string.</returns>
        public static string GenerateSalt(int size = 16)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Hashes a password using Argon2id with the provided salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use for hashing (Base64 encoded).</param>
        /// <returns>The Argon2id hashed password (Base64 encoded).</returns>
        public static string HashPassword(string password, string salt, int degreeOfParallelism = 4, int iterations = 4, int memorySize = 1024 * 64)
        {
            // Convert the Base64-encoded salt to bytes.
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Create an instance of Argon2id with the password bytes.
            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = saltBytes,
                // Configure parameters as needed (tuning is recommended for your environment).
                DegreeOfParallelism = degreeOfParallelism,   // Number of threads to use
                Iterations = iterations,            // Number of iterations
                MemorySize = memorySize     // Memory size in kilobytes (e.g., 64 MB)
            };

            // Compute the hash. Here, the hash length is set to 16 bytes (128 bits); adjust as needed.
            byte[] hashBytes = argon2.GetBytes(16);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies a password against a stored Argon2id hash using the provided salt.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hashedPassword">The stored hashed password (Base64 encoded).</param>
        /// <param name="salt">The salt used (Base64 encoded).</param>
        /// <returns>True if the password is valid, false otherwise.</returns>
        public static bool VerifyPassword(string password, string? hashedPassword, string? salt, int? degreeOfParallelism , int? iterations , int? memorySize)
        {
            // Check if the hashed password and salt are not null.
            // If either is null, the password cannot be verified.
            // This is a security measure to prevent timing attacks.
            if (hashedPassword == null || salt == null)
            {
                return false;
            }
            
            // Re-hash the provided password using the stored salt.
            string computedHash = HashPassword(password, salt, degreeOfParallelism ?? 4, iterations ?? 4, memorySize ?? 1024 * 64);
            // Compare the computed hash with the stored hash.
            return hashedPassword == computedHash;
        }
    }
}
