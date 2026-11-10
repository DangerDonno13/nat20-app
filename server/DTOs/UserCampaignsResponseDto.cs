namespace Nat20Server.DTOs
{
    public class UserCampaignsResponseDto
    {
        public List<CampaignDto> OwnedCampaigns { get; set; } = new List<CampaignDto>();
        public List<CampaignDto> SharedCampaigns { get; set; } = new List<CampaignDto>();
    }
}
