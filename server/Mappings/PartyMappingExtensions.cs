using Nat20Server.DTOs;
using Nat20Server.Entities;

namespace Nat20Server.Mappings
{
    public static class PartyMappingExtensions
    {
        public static PartyDto ToDto(this Party party)
        {
            return new PartyDto
            {
                Id = party.Id,
                CharacterIds = party.CharacterIds
            };
        }
    }
}
