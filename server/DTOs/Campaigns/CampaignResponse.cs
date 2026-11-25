using Nat20Server.Entities;

namespace Nat20Server.DTOs.Campaigns
{
    public class CampaignResponse
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public CampaignSetting Setting { get; set; } = CampaignSetting.Unspecified;
        public CampaignStatus Status { get; set; } = CampaignStatus.Unspecified;
        public DateTime CreatedAt { get; set; }
        public string OwnerId { get; set; } = null!;
        public List<string> PlayerIds { get; set; } = new List<string>();
    }
}
