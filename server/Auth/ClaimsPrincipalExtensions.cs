using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Nat20Server.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the authenticated user's id from the "sub" claim of their JWT.
        /// </summary>
        /// <param name="principal">The current user (ControllerBase.User).</param>
        /// <returns>The user's id, or null if the claim is missing.</returns>
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        }
    }
}