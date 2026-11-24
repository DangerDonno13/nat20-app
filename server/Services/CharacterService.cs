using MongoDB.Driver;
using Nat20Server.DTOs;
using Nat20Server.Entities;

namespace Nat20Server.Services
{
    public class CharacterService
    {
        private readonly IMongoCollection<Character> _charactersCollection;

        public CharacterService(IMongoDatabase database)
        {
            _charactersCollection = database.GetCollection<Character>("Characters");
        }

        public async Task<Character?> GetCharacterByIdAsync(string id)
        {
            return await _charactersCollection.Find(character => character.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Character>> GetCharactersByIdsAsync(IEnumerable<string> ids)
        {
            return await _charactersCollection.Find(c => c.Id != null && ids.Contains(c.Id)).ToListAsync();
        }

        public async Task<Character> CreateCharacterAsync(CharacterDto character, string ownerId)
        {
            var characterEntity = new Character
            {
                OwnerId = ownerId,
                Name = character.Name,
                Race = character.Race,
                Class = character.Class,
                Background = character.Background,
                Inventory = character.Inventory,
                Spells = character.Spells,
                Skills = character.Skills
            };
            await _charactersCollection.InsertOneAsync(characterEntity);
            return characterEntity;
        }
    }
}
