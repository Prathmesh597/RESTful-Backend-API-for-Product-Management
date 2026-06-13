using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        // 1. Initializes the authentication service with required dependencies
        public AuthService(IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // 2. Checks if the requested username is already taken and throws an exception if so
            if (await _authRepository.UsernameExistsAsync(dto.Username))
                throw new AppException("Username already exists.", 400);

            // 3. Verifies that the provided email address is not already registered
            if (await _authRepository.EmailExistsAsync(dto.Email))
                throw new AppException("Email already exists.", 400);

            // 4. Securely hashes the user's password using the BCrypt algorithm
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 5. Constructs a new user entity with the provided details and an initial refresh token
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = dto.Role,
                CreatedOn = DateTime.UtcNow,
                RefreshToken = _jwtTokenGenerator.GenerateRefreshToken(),
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
            };

            // 6. Persists the newly created user to the database
            var created = await _authRepository.CreateAsync(user);

            // 7. Generates a JWT access token for the newly registered user
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(
                created.Id.ToString(),
                created.Username,
                created.Role);

            // 8. Returns the authentication payload containing tokens and user information
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = created.RefreshToken!,
                Username = created.Username,
                Role = created.Role
            };

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // 9. Attempts to retrieve the user record by their username
            var user = await _authRepository.GetByUsernameAsync(dto.Username);

            // 10. Rejects the authentication attempt if the user is not found
            if (user == null)
                throw new AppException("Invalid username or password.", 401);

            // 11. Validates the provided password against the stored hash
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new AppException("Invalid username or password.", 401);

            // 12. Issues a new access token for the authenticated user
            var accessToken = _jwtTokenGenerator.GenerateAccessToken(
                user.Id.ToString(),
                user.Username,
                user.Role);

            // 13. Generates a fresh refresh token and extends its expiration date
            user.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // 14. Updates the user record with the new refresh token details
            await _authRepository.UpdateAsync(user);

            // 15. Delivers the updated tokens and user details back to the client
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
                Username = user.Username,
                Role = user.Role
            };

        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            // 16. Locates the user associated with the provided refresh token
            var user = await _authRepository.GetByRefreshTokenAsync(dto.RefreshToken);

            // 17. Fails the request if no user matches the token
            if (user == null)
                throw new AppException("Invalid refresh token.", 401);

            // 18. Verifies that the refresh token has not expired
            if (user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new AppException("Refresh token expired.", 401);

            // 19. Creates a replacement access token for continued access
            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(
                user.Id.ToString(),
                user.Username,
                user.Role);

            // 20. Rotates the refresh token for security purposes
            user.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // 21. Saves the newly rotated token to the database
            await _authRepository.UpdateAsync(user);

            // 22. Returns the refreshed set of authentication credentials
            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = user.RefreshToken,
                Username = user.Username,
                Role = user.Role
            };

        }

        public async Task RevokeTokenAsync(string username)
        {
            // 23. Fetches the target user by username to invalidate their session
            var user = await _authRepository.GetByUsernameAsync(username);

            // 24. Throws a not found exception if the specified user does not exist
            if (user == null)
                throw new NotFoundException("User", 0);

            // 25. Clears the refresh token data to force a re-authentication
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            // 26. Commits the revocation to the database
            await _authRepository.UpdateAsync(user);

        }

    }

}   