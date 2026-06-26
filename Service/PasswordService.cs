using System;
using System.Security.Cryptography;
using System.Text;

namespace InternalBaseWpf.Service
{
    public static class PasswordService
    {
        public static (string hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            string hash = ComputeHash(password, salt);
            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string hash, string salt)
        {
            return ComputeHash(password, salt) == hash;
        }

        public static string GeneratePassword(int length = 10)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            var sb = new StringBuilder();
            byte[] randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[randomBytes[i] % chars.Length]);
            }
            return sb.ToString();
        }

        private static string ComputeHash(string password, string salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                Encoding.UTF8.GetBytes(salt),
                10000,
                HashAlgorithmName.SHA256);
            return Convert.ToBase64String(pbkdf2.GetBytes(32));
        }
    }
}
