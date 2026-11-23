using MongoDB.Driver;
using Nat20Server.DTOs;
using Nat20Server.Entities;

namespace Nat20Server.Services
{
    public class PartyService
    {
        private readonly IMongoCollection<Party> _partiesCollection;
        
        public PartyService(IMongoDatabase database)
        {
            _partiesCollection = database.GetCollection<Party>("Parties");
        }

        public async Task<Party?> GetPartyByIdAsync(string id)
        {
            return await _partiesCollection.Find(party => party.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Party> CreatePartyAsync(PartyDto party)
        {
            var partyEntity = new Party
            {
                Id = party.Id,
                CharacterIds = party.CharacterIds
            };

            await _partiesCollection.InsertOneAsync(partyEntity);
            return partyEntity;
        }

        public async Task<Party?> AddCharacterAsync(string partyId, string characterId)
        {
            var filter = Builders<Party>.Filter.Eq(p => p.Id, partyId);
            var update = Builders<Party>.Update.AddToSet(p => p.CharacterIds, characterId);

            var options = new FindOneAndUpdateOptions<Party>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _partiesCollection.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}
