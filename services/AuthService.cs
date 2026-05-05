using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;

namespace WebApplication1.services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDB _dbContext;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _durationMinutes;

        public AuthService(ApplicationDB dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _secretKey = configuration["JwtSettings:Key"] ?? "Default-Secret-Key-Change-Me-123456";
            _issuer = configuration["JwtSettings:Issuer"] ?? "TaskApi";
            _audience = configuration["JwtSettings:Audience"] ?? "TaskApiUsers";
            _durationMinutes = int.TryParse(configuration["JwtSettings:DurationMinutes"], out var minutes) ? minutes : 60;
        }

        public async Task<AppUser?> ValidateUserCredentialsAsync(string email, string password)
        {
            var normalizedEmail = email.Trim().ToLower();
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);
            if (user == null)
            {
                return null;
            }

            return VerifyPassword(user.PasswordHash, password) ? user : null;
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_durationMinutes);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            return hashedPassword == HashPassword(password);
        }
    }
}
