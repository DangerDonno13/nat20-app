using MongoDB.Driver;
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

        public async Task<CharacterResponse?> UpdateCharacterAsync(string id, UpdateCharacterRequest request)
        {
            var updateBuilder = Builders<Character>.Update;
            var updateDefinition = new List<UpdateDefinition<Character>>();

            if (request.Name != null)
                updateDefinition.Add(updateBuilder.Set(c => c.Name, request.Name));
            if (request.Race != null)
                updateDefinition.Add(updateBuilder.Set(c => c.Race, request.Race));
            if (request.Class != null)
                updateDefinition.Add(updateBuilder.Set(c => c.Class, request.Class));
            if (request.Background != null)
                updateDefinition.Add(updateBuilder.Set(c => c.Background, request.Background));
            if (request.Inventory != null)
                updateDefinition.Add(updateBuilder.Set(c => c.Inventory, request.Inventory));
            if (request.Spells != null)
                updateDefinition.Add(updateBuilder.Set(c => c.Spells, request.Spells));
            if (request.Skills != null)
                updateDefinition.Add(updateBuilder.Set(c => c.Skills, request.Skills));

            if (updateDefinition.Count == 0)
                return null;

            var result = await _charactersCollection.FindOneAndUpdateAsync(
                character => character.Id == id,
                updateBuilder.Combine(updateDefinition),
                new FindOneAndUpdateOptions<Character> { ReturnDocument = ReturnDocument.After }
            );

            return result?.ToResponse();
        }
    }
}
