using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces
{
    public interface IAuthRepository
    {
        // 1. Retrieves a specific user record from the database using their unique username
        Task<User?> GetByUsernameAsync(string username);

        // 2. Looks up a user account associated with the provided active refresh token
        Task<User?> GetByRefreshTokenAsync(string refreshToken);

        // 3. Persists a newly registered user entity into the database
        Task<User> CreateAsync(User user);

        // 4. Saves modifications made to an existing user's profile or security credentials
        Task UpdateAsync(User user);

        // 5. Checks the database to determine if a chosen username is already taken
        Task<bool> UsernameExistsAsync(string username);

        // 6. Verifies whether an email address is already registered to another account
        Task<bool> EmailExistsAsync(string email);

    }

}