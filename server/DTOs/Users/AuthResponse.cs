using Nat20Server.DTOs.User;

namespace Nat20Server.DTOs.Users
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public UserPublicDto User { get; set; } = null!;
    }
}
