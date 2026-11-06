using RESTcats.Models;

namespace RESTcats.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate a user by username and password
        /// </summary>
        /// <returns>User object if authentication successful, null otherwise</returns>
        Task<User?> AuthenticateUserAsync(string username, string password);
    }
}
