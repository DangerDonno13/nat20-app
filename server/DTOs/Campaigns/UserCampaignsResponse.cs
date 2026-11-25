namespace Nat20Server.DTOs.Campaigns
{
    public class UserCampaignsResponse
    {
        public List<CampaignResponse> OwnedCampaigns { get; set; } = new List<CampaignResponse>();
        public List<CampaignResponse> SharedCampaigns { get; set; } = new List<CampaignResponse>();
    }
}
