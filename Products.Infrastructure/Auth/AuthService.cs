using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Products.Application.DTOs.Auth;
using Products.Application.Interfaces;
using Products.Application.Interfaces.Persistence;
using Products.Application.Interfaces.Services;
using Products.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Products.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var existing = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existing != null)
                throw new InvalidOperationException("Username already taken");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                Role = "User"
            };

            var createdUser = await _userRepository.CreateAsync(user);
            var tokenData = GenerateToken(createdUser);

            return new AuthResponseDto
            {
                Token = tokenData.TokenString,
                ExpiresAt = tokenData.ExpiresAt,
                Username = createdUser.Username,
                Role = createdUser.Role
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null) return null;

            var verified = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!verified) return null;

            var tokenData = GenerateToken(user);

            return new AuthResponseDto
            {
                Token = tokenData.TokenString,
                ExpiresAt = tokenData.ExpiresAt,
                Username = user.Username,
                Role = user.Role
            };
        }

        private (string TokenString, DateTime ExpiresAt) GenerateToken(User user)
        {
            var jwt = _configuration.GetSection("Jwt");

            var key = jwt["Key"];
            if (string.IsNullOrEmpty(key))
                throw new ApplicationException("JWT Key is missing in configuration!");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expires = DateTime.UtcNow.AddMinutes(jwt.GetValue<int>("DurationInMinutes"));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
