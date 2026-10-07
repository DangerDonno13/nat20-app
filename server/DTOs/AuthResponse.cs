namespace Nat20Server.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public UserPublicDto User { get; set; } = null!;
    }
}
