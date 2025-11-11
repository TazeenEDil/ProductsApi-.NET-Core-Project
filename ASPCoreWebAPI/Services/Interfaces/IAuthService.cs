using ProductsApi.DTOs.Auth;

namespace ProductsApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ProductsApi.DTOs.Auth.AuthResponseDto> RegisterAsync(ProductsApi.DTOs.Auth.RegisterRequestDto dto);
        Task<ProductsApi.DTOs.Auth.AuthResponseDto?> LoginAsync(ProductsApi.DTOs.Auth.LoginRequestDto dto);
    }
}
