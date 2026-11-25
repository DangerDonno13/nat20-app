using MongoDB.Driver;
using Nat20Server.DTOs.Party;
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

        public async Task<PartyResponse?> GetPartyByIdAsync(string id)
        {
            var party = await _partiesCollection.Find(party => party.Id == id).FirstOrDefaultAsync();
            return party?.ToResDto();
        }

        public async Task<PartyResponse> CreatePartyAsync(CreatePartyRequest request)
        {
            var partyEntity = new Party
            {
                CampaignId = request.CampaignId,
                CharacterIds = request.CharacterIds ?? []
            };

            await _partiesCollection.InsertOneAsync(partyEntity);
            return partyEntity.ToResDto();
        }

        public async Task<PartyResponse?> AddCharacterAsync(string partyId, string characterId)
        {
            var filter = Builders<Party>.Filter.Eq(p => p.Id, partyId);
            var update = Builders<Party>.Update.AddToSet(p => p.CharacterIds, characterId);

            var options = new FindOneAndUpdateOptions<Party>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedParty = await _partiesCollection.FindOneAndUpdateAsync(filter, update, options);
            return updatedParty?.ToResDto();
        }
    }
}
