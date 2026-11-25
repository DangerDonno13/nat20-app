using Nat20Server.DTOs.Party;
using Nat20Server.Entities;

namespace Nat20Server.Mappings
{
    public static class PartyMappingExtensions
    {
        public static PartyResponse ToResDto(this Party party)
        {
            return new PartyResponse
            {
                Id = party.Id,
                CampaignId = party.CampaignId,
                CharacterIds = party.CharacterIds
            };
        }
    }
}
