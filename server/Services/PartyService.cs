using MongoDB.Driver;
using Nat20Server.DTOs.Parties;
using Nat20Server.Entities;
using Nat20Server.Mappings;

namespace Nat20Server.Services
{
    public class PartyService
    {
        private readonly IMongoCollection<Party> _partiesCollection;

        public PartyService(IMongoDatabase database)
        {
            _partiesCollection = database.GetCollection<Party>("Parties");
        }

        /// <summary>
        /// Gets a party by its ID.
        /// </summary>
        /// <param name="id">The ID of the party to retrieve.</param>
        /// <returns>The party if found, otherwise null.</returns>
        public async Task<PartyResponse?> GetPartyByIdAsync(string id)
        {
            var party = await _partiesCollection.Find(party => party.Id == id).FirstOrDefaultAsync();
            return party?.ToResponse();
        }

        /// <summary>
        /// Creates a new party with the specified campaign ID and an empty list of character IDs.
        /// </summary>
        /// <param name="request">The request containing the party details.</param>
        /// <returns>The created party as a PartyResponse object.</returns>
        public async Task<PartyResponse> CreatePartyAsync(CreatePartyRequest request)
        {
            var partyEntity = new Party
            {
                CampaignId = request.CampaignId,
                CharacterIds = []
            };

            await _partiesCollection.InsertOneAsync(partyEntity);
            return partyEntity.ToResponse();
        }

        /// <summary>
        /// Adds a character to a party by updating the party's CharacterIds list.
        /// </summary>
        /// <param name="partyId">The ID of the party to update.</param>
        /// <param name="characterId">The ID of the character to add.</param>
        /// <returns>The updated party as a PartyResponse object, or null if not found.</returns>
        public async Task<PartyResponse?> AddCharacterAsync(string partyId, string characterId)
        {
            var filter = Builders<Party>.Filter.Eq(p => p.Id, partyId);
            var update = Builders<Party>.Update.AddToSet(p => p.CharacterIds, characterId);

            var options = new FindOneAndUpdateOptions<Party>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedParty = await _partiesCollection.FindOneAndUpdateAsync(filter, update, options);
            return updatedParty?.ToResponse();
        }
    }
}
