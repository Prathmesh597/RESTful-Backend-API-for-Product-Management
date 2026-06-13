using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Interfaces
{
    public interface IAuthService
    {
        // 1. Processes a new user registration request and returns the initial authentication tokens
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

        // 2. Authenticates user credentials and generates access and refresh tokens upon success
        Task<AuthResponseDto> LoginAsync(LoginDto dto);

        // 3. Issues a new set of authentication tokens using a valid refresh token
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);

        // 4. Invalidates the active refresh token for a specific user to terminate their session
        Task RevokeTokenAsync(string username);

    }

}