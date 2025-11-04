using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductsApi.DTOs.Auth;
using ProductsApi.Models;
using ProductsApi.Repositories.Interfaces;
using ProductsApi.Services.Interfaces;

namespace ProductsApi.Services
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
            try
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
            catch (InvalidOperationException)
            {
                throw; // known validation issue
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to register user", ex);
            }
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            try
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
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to login user", ex);
            }
        }

        private (string TokenString, DateTime ExpiresAt) GenerateToken(User user)
        {
            try
            {
                var jwt = _configuration.GetSection("Jwt");

                var jwtKey = jwt["Key"];
                if (string.IsNullOrWhiteSpace(jwtKey))
                    throw new Exception("JWT Key missing in configuration!");

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
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
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to generate JWT token", ex);
            }
        }
    }
}
