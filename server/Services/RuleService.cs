#region Usings
using System.Collections.Generic;

using Nat20Server.DTOs;
#endregion

namespace Nat20Server.Services
{
    public sealed class RuleService(IWebHostEnvironment environment)
    {
        private readonly string _rulesDir = Path.Combine(
            environment.ContentRootPath,
            "Content",
            "Rules");

        public async Task<RuleChapterDto?> GetRuleChapterAsync(string slug)
        {
            // Validate slug before searching content directory for it.
            if (string.IsNullOrEmpty(slug) ||
                slug.Any(c => !char.IsAsciiLetterOrDigit(c) && c != '-' && c != '_')) return null;

            var path = Path.Combine(_rulesDir, $"{slug}.md");
            if (!File.Exists(path)) return null;

            var content = await File.ReadAllTextAsync(path);
            return new(slug, content);
        }
    }
}
