using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TimeAttendance.Models;

namespace TimeAttendance
{
    public static class HashUtility
    {
        public static string GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            var saleByte = Convert.FromBase64String(salt);
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saleByte,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

        public static bool PasswordIsValid(string password, User user)
        {
            if (string.IsNullOrEmpty(password) || user == null) return false;
            var hashed = HashPassword(password, user.Salt);
            return hashed == user.Password;
        }
    }
}