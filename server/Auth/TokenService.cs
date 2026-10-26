using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Nat20Server.Entities;

namespace Nat20Server.Auth
{
    /// <summary>
    /// Creates signed JWT access tokens for authenticated users.
    /// </summary>
    public class TokenService
    {
        private readonly JwtSettings _settings;
        private readonly JsonWebTokenHandler _handler = new();

        public TokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// Generates a signed JWT for the given user. The user's id is stored in the "sub" claim.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>The serialized JWT.</returns>
        public string GenerateToken(User user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                throw new InvalidOperationException("Cannot issue a token for a user without an id.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

            var claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id,
                [JwtRegisteredClaimNames.Email] = user.Email,
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString()
            };

            if (!string.IsNullOrEmpty(user.Username))
            {
                claims["username"] = user.Username;
            }

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                Claims = claims,
                Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            return _handler.CreateToken(descriptor);
        }
    }
}