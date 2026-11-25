using MongoDB.Driver;
using Nat20Server.DTOs.Character;
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
            return character?.ToResDto();
        }

        public async Task<CharacterResponse> CreateCharacterAsync(CreateCharacterRequest req, string ownerId)
        {
            var characterEntity = new Character
            {
                OwnerId = ownerId,
                Name = req.Name,
                Race = req.Race,
                Class = req.Class,
                Background = req.Background,
                Inventory = req.Inventory ?? [],
                Spells = req.Spells,
                Skills = req.Skills
            };
            await _charactersCollection.InsertOneAsync(characterEntity);
            return characterEntity.ToResDto();
        }
    }
}
