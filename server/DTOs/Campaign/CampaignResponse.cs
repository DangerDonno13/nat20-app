using Nat20Server.Entities;
using System.ComponentModel.DataAnnotations;

namespace Nat20Server.DTOs.Campaign
{
    public class CampaignResponse
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public CampaignSetting Setting { get; set; } = CampaignSetting.Unspecified;
        public CampaignStatus Status { get; set; } = CampaignStatus.Unspecified;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string OwnerId { get; set; } = null!;
        public List<string> PlayerIds { get; set; } = new List<string>();
    }
}
