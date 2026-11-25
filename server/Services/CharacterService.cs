using MongoDB.Driver;
using Nat20Server.DTOs.Character;
using Nat20Server.DTOs.Characters;
using Nat20Server.Entities;
using Nat20Server.Mappings;

namespace Nat20Server.Services
{
    public class CharacterService
    {
        private readonly IMongoCollection<Character> _charactersCollection;

        public CharacterService(IMongoDatabase database)
        {
            _charactersCollection = database.GetCollection<Character>("Characters");
        }

        public async Task<CharacterResponse?> GetCharacterByIdAsync(string id)
        {
            var character = await _charactersCollection.Find(character => character.Id == id).FirstOrDefaultAsync();
            return character?.ToResponse();
        }

        public async Task<CharacterResponse> CreateCharacterAsync(CreateCharacterRequest request, string ownerId)
        {
            var characterEntity = new Character
            {
                OwnerId = ownerId,
                Name = request.Name,
                Race = request.Race,
                Class = request.Class,
                Background = request.Background,
                Inventory = request.Inventory ?? [],
                Spells = request.Spells,
                Skills = request.Skills
            };
            await _charactersCollection.InsertOneAsync(characterEntity);
            return characterEntity.ToResponse();
        }
    }
}
