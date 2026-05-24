using EdAnalytics.Application.DTOs;

namespace EdAnalytics.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> AuthenticateAsync(LoginDto login);
        Task<bool> RegisterAsync(RegisterDto register);
    }
}

