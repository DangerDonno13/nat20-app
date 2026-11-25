using Nat20Server.DTOs.Campaigns;
using Nat20Server.Entities;

namespace Nat20Server.Mappings
{
    public static class CampaignMappingExtensions
    {
        public static CampaignResponse ToResponse(this Campaign campaign)
        {
            return new CampaignResponse
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Description = campaign.Description,
                Setting = campaign.Setting,
                Status = campaign.Status,
                OwnerId = campaign.OwnerId,
                PlayerIds = campaign.PlayerIds,
                CreatedAt = campaign.CreatedAt,
            };
        }
    }
}
