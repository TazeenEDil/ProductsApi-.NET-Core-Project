using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Application.DTOs.Auth;
using Products.Application.Interfaces;
using Products.Application.Interfaces.Services;

namespace Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // Register (User or Admin)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.Role))
            {
                dto.Role = "User"; // default role
            }
            else if (dto.Role != "User" && dto.Role != "Admin")
            {
                return BadRequest(new { message = "Role must be either 'User' or 'Admin'" });
            }

            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(result);
        }

        
        
    }
}
