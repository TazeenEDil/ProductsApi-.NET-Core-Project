namespace ProductsApi.DTOs.Auth
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}
