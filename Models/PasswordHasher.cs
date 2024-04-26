
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace AppsDevCoffee.Models
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with PBKDF2
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));
            // Combine the salt and hashed password
            string combinedHash = $"{Convert.ToBase64String(salt)}:{hashedPassword}";

            return combinedHash;
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // Extract the salt and hash from the combined hash
            string[] parts = hashedPassword.Split(':');
            byte[] salt = Convert.FromBase64String(parts[0]);
            string storedHash = parts[1];

            // Hash the provided password with the same salt
            string hashedProvidedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            // Compare the stored hash with the hash of the provided password
            return storedHash == hashedProvidedPassword;
        }
    }
}
