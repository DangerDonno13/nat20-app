using Nat20Server.DTOs.Character;
using Nat20Server.Entities;

namespace Nat20Server.Mappings
{
    public static class CharacterMappingExtensions
    {
        public static CharacterResponse ToResDto(this Character character)
        {
            return new CharacterResponse
            {
                Id = character.Id,
                OwnerId = character.OwnerId,
                Name = character.Name,
                Race = character.Race,
                Class = character.Class,
                Background = character.Background,
                Inventory = character.Inventory,
                Spells = character.Spells,
                Skills = character.Skills
            };
        }
    }
}
