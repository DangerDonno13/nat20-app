using System.ComponentModel.DataAnnotations;

namespace Nat20Server.DTOs
{
    public class CampaignDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public string Setting { get; set; } = string.Empty;
    }
}
