using Aesthetic.Application.Common.Interfaces.Authentication;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Aesthetic.Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // Simple implementation for demo, use Identity or BCrypt in production if possible
            // But since I have Identity Core, I can use it? No, keep it simple and clean without full Identity dependency if not needed.
            // Actually, let's use BCrypt.Net-Next if I can add it, or just use standard PBKDF2.
            // Using standard PBKDF2 here.
            
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('.', 2);
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return storedHash == hashed;
        }
    }
}