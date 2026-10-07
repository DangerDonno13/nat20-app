namespace Nat20Server.DTOs
{
    public class UserPublicDto
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; } = null!;
    }
}
