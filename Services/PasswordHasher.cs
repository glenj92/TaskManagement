using System.Security.Cryptography;
using System.Text;

namespace TaskManagement.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            var computedHash = HashPassword(password);
            return computedHash == storedHash;
        }
    }
}
