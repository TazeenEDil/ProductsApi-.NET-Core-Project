using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Products.Application.DTOs.Auth;


namespace Products.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
    }
}
