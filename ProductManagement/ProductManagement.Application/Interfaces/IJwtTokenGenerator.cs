using System.Security.Claims;

namespace ProductManagement.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        // 1. Defines the contract for generating a short-lived access token containing user claims
        string GenerateAccessToken(string userId, string username, string role);

        // 2. Specifies the method for creating a secure, cryptographically random refresh token
        string GenerateRefreshToken();

        // 3. Outlines the function to securely extract user identity claims from a previously expired token
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    }

}