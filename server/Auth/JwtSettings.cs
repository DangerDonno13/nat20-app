namespace Nat20Server.Auth
{
    /// <summary>
    /// Settings for JWT token generation and validation. These settings are loaded from configuration.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpiryMinutes { get; set; } = 60;
    }
}