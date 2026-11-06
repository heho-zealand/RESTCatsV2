using System.Security.Cryptography;
using System.Text;
using RESTcats.Models;

namespace RESTcats.Services
{
    public class AuthService : IAuthService
    {
        private readonly CatsDbContext _context;

        public AuthService(CatsDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            // Find user in database
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return null;

            // Verify password hash
            if (!VerifyPasswordHash(password, user.PasswordHash))
                return null;

            return user;
        }

        /// <summary>
        /// Hash a password using PBKDF2
        /// </summary>
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashWithSalt = new byte[36];
            Array.Copy(salt, 0, hashWithSalt, 0, 16);
            Array.Copy(hash, 0, hashWithSalt, 16, 20);

            return Convert.ToBase64String(hashWithSalt);
        }

        /// <summary>
        /// Verify a password against its hash
        /// </summary>
        private static bool VerifyPasswordHash(string password, string hash)
        {
            byte[] hashWithSalt = Convert.FromBase64String(hash);
            byte[] salt = new byte[16];
            Array.Copy(hashWithSalt, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash2 = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashWithSalt[i + 16] != hash2[i])
                    return false;
            }

            return true;
        }
    }
}
