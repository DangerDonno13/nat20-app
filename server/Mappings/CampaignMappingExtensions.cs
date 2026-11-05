using Nat20Server.DTOs;
using Nat20Server.Entities;

namespace Nat20Server.Mappings
{
    public static class CampaignMappingExtensions
    {
        public static CampaignDto ToDto(this Campaign campaign)
        {
            return new CampaignDto
            {
                Name = campaign.Name,
                Description = campaign.Description,
                Setting = campaign.Setting,
                Status = campaign.Status.ToString()
            };
        }
    }
}
