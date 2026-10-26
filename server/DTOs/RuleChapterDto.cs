namespace Nat20Server.DTOs
{
    /// <summary>
    /// A chapter from the rules for Dungeons and Dragons DTO.
    /// </summary>
    /// <param name="Slug">
    /// The url-friendly identifier for the rule chapter (e.g., "roles").
    /// </param>
    /// <param name="Content">The rule chapter's raw text content.</param>
    public sealed record RuleChapterDto(string Slug, string Content);
}
