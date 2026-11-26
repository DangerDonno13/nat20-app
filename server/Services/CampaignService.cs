using MongoDB.Driver;
using Nat20Server.DTOs.Campaigns;
using Nat20Server.Entities;
using Nat20Server.Mappings;

namespace Nat20Server.Services
{
    /// <summary>
    /// Service for managing campaign-related operations, including retrieving and creating campaigns in the database.
    /// </summary>
    public class CampaignService
    {
        private readonly IMongoCollection<Campaign> _campaignsCollection;
        public CampaignService(IMongoDatabase database)
        {
            _campaignsCollection = database.GetCollection<Campaign>("Campaigns");
        }

        /// <summary>
        /// Retrieves a campaign from the database based on its ID.
        /// </summary>
        /// <param name="id">The ID of the campaign to retrieve.</param>
        /// <returns>The campaign if found, otherwise null.</returns>
        public async Task<CampaignResponse?> GetCampaignByIdAsync(string id)
        {
            var campaign = await _campaignsCollection.Find(campaign => campaign.Id == id).FirstOrDefaultAsync();
            return campaign?.ToResponse();
        }

        /// <summary>
        /// Retrieves all campaigns associated with a specific user, including both owned and shared campaigns.
        /// </summary>
        /// <param name="userId">The ID of the user for whom to retrieve campaigns.</param>
        /// <returns>A response containing the user's owned and shared campaigns.</returns>
        public async Task<UserCampaignsResponse> GetCampaignsForUserAsync(string userId)
        {
            var campaigns = await _campaignsCollection.Find(c => c.OwnerId == userId || c.PlayerIds.Contains(userId)).ToListAsync();

            return new UserCampaignsResponse
            {
                OwnedCampaigns = campaigns.Where(c => c.OwnerId == userId).Select(c => c.ToResponse()).ToList(),
                SharedCampaigns = campaigns.Where(c => c.PlayerIds.Contains(userId)).Select(c => c.ToResponse()).ToList()
            };
        }

        /// <summary>
        /// Creates a new campaign in the database.
        /// </summary>
        /// <param name="campaign">The campaign to create.</param>
        /// <param name="ownerId">The ID of the user who owns the campaign.</param>
        public async Task<CampaignResponse> CreateCampaignAsync(CreateCampaignRequest request, string ownerId)
        {
            var campaignEntity = new Campaign
            {
                Name = request.Name,
                Description = request.Description,
                Setting = request.Setting,
                OwnerId = ownerId,
                Status = request.Status
            };
            await _campaignsCollection.InsertOneAsync(campaignEntity);
            return campaignEntity.ToResponse();
        }
        
        /// <summary>
        /// Shares a campaign with a user.
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to share.</param>
        /// <param name="userId">The ID of the user with whom to share the campaign.</param>
        public async Task ShareCampaign(string campaignId, string userId)
        {
            var update = Builders<Campaign>.Update.AddToSet(c => c.PlayerIds, userId);
            await _campaignsCollection.UpdateOneAsync(c => c.Id == campaignId, update);
        }
    }
}
