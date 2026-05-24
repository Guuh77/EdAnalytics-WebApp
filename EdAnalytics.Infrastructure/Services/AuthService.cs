using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EdAnalytics.Application.DTOs;
using EdAnalytics.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EdAnalytics.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private static readonly object _lock = new();

        private static readonly Dictionary<string, (string Password, string Role)> _users = new()
        {
            { "admin@edanalytics.com", ("Admin@123", "Admin") },
            { "professor@edanalytics.com", ("Prof@123", "Professor") },
            { "aluno@edanalytics.com", ("Aluno@123", "Aluno") }
        };

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> RegisterAsync(RegisterDto register)
        {
            lock (_lock)
            {
                if (_users.ContainsKey(register.Email))
                {
                    return Task.FromResult(false);
                }

                _users[register.Email] = (register.Password, register.Role);
                return Task.FromResult(true);
            }
        }


        public Task<TokenResponseDto?> AuthenticateAsync(LoginDto login)
        {
            if (!_users.TryGetValue(login.Email, out var user) || user.Password != login.Password)
            {
                return Task.FromResult<TokenResponseDto?>(null);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, login.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name, login.Email)
                }),
                Expires = expiration,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult<TokenResponseDto?>(new TokenResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expiration
            });
        }
    }
}
