using System.ComponentModel.DataAnnotations;

namespace Nat20Server.DTOs.Campaigns
{
    public class ShareCampaignRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
    }
}
