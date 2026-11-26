using Nat20Server.DTOs.Parties;
using Nat20Server.Entities;

namespace Nat20Server.Mappings
{
    public static class PartyMappingExtensions
    {
        /// <summary>
        /// Maps a Party to a PartyResponse DTO.
        /// </summary>
        public static PartyResponse ToResponse(this Party party)
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
