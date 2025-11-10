namespace Products.Application.DTOs.Auth

{
    public class RegisterRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string? Role { get; set; } // Admin or User
    }
}
