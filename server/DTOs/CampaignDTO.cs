using Nat20Server.Entities;
using System.ComponentModel.DataAnnotations;

namespace Nat20Server.DTOs
{
    public class CampaignDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public CampaignSetting Setting { get; set; } = CampaignSetting.Unspecified;
        public CampaignStatus Status { get; set; } = CampaignStatus.Unspecified;
    }
}
