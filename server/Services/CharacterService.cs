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

        /// <summary>
        /// Gets a character by its ID from the database and returns it as a CharacterResponse object. If the character is not found, it returns null.
        /// </summary>
        /// <param name="id">The ID of the character to retrieve.</param>
        /// <returns>The character if found, otherwise null.</returns>
        public async Task<CharacterResponse?> GetCharacterByIdAsync(string id)
        {
            var character = await _charactersCollection.Find(character => character.Id == id).FirstOrDefaultAsync();
            return character?.ToResponse();
        }

        /// <summary>
        /// Creates a character in the database based on the provided CreateCharacterRequest and ownerId, and returns the created character as a CharacterResponse object.
        /// </summary>
        /// <param name="request">The request containing the character details.</param>
        /// <param name="ownerId">The ID of the user who owns the character.</param>
        /// <returns>The created character as a CharacterResponse object.</returns>
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

        /// <summary>
        /// Updates an existing Character with the provided ID based on the UpdateCharacterRequest.
        /// Only the fields that are not null in the request will be updated. If no fields are provided for update, it returns null.
        /// Otherwise, it returns the updated character as a CharacterResponse object.
        /// </summary>
        /// <param name="id">The ID of the character to update.</param>
        /// <param name="request">The request containing the updated character details.</param>
        /// <returns>The updated character as a CharacterResponse object, or null if no updates were made.</returns>
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
