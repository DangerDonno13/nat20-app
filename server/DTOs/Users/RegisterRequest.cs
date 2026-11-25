using System.ComponentModel.DataAnnotations;

namespace Nat20Server.DTOs.Users
{
    // This DTO is used for user registration. It includes necessary fields for creating a new user account.
    public class RegisterRequest
    {
        [Required, StringLength(32, MinimumLength = 3)]
        public string Username { get; set; } = null!;

        [Required, EmailAddress, StringLength(254)]
        public string Email { get; set; } = null!;

        [Required, StringLength(128, MinimumLength = 8)]
        public string Password { get; set; } = null!;
    }
}
