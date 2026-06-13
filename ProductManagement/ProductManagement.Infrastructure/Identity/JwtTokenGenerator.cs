using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Application.Interfaces;

namespace ProductManagement.Infrastructure.Identity
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        // 1. Initializes the token generator with the required JWT configuration settings
        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        // 2. Creates a short-lived access token containing user identity and role information
        public string GenerateAccessToken(string userId, string username, string role)
        {
            // 3. Bundles the user details and a unique token identifier into a claims array
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 4. Converts the secret key string into a byte array for cryptographic use
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            // 5. Prepares the signing credentials using the generated key and the HMAC SHA256 algorithm
            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            // 6. Constructs the actual JWT object with the required issuer, audience, claims, and expiration details
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials
            );

            // 7. Serializes the token object into a secure, compact string format for client transmission
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // 8. Generates a secure, cryptographically random string to act as a refresh token
        public string GenerateRefreshToken()
        {
            // 9. Allocates a 64-byte array to hold the random data
            var randomBytes = new byte[64];

            // 10. Fills the byte array with cryptographically strong random values
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            // 11. Converts the random bytes into a Base64 encoded string for safe storage and transmission
            return Convert.ToBase64String(randomBytes);
        }

        // 12. Extracts user claims from an existing token, bypassing the standard expiration check
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            // 13. Configures validation rules, specifically disabling the lifetime validation to allow reading expired tokens
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
            };

            // 14. Validates the provided token against the configured parameters and extracts its security payload
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(
                token, tokenValidationParameters, out var securityToken);

            // 15. Verifies that the parsed token is a valid JWT and was signed with the expected HMAC SHA256 algorithm
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            // 16. Returns the successfully extracted claims principal containing the user's data payload
            return principal;
        }

    }

}