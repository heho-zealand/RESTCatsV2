using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RESTcats.Models;
using RESTcats.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTcats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration config, IAuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            // Authenticate user using the auth service
            var user = await _authService.AuthenticateUserAsync(login.Username, login.Password);

            if (user is null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = GenerateJwtToken(user.Username, user.Role);
            return Ok(new { token, role = user.Role });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims are the pieces of information "baked" into the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role) // You can add roles here
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"], // Udsteder: den der har udstedt tokenet - her RestCats
                audience: jwtSettings["Audience"], // Modtager: dem der må benytte tokenet (alle andre afvises) - tildeles værdien "JustAClient" 
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token er valid 1 time
                signingCredentials: creds // Signatur
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Helper class to receive JSON data
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
