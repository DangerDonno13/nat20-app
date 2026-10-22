using MongoDB.Driver;
using Nat20Server.DTOs;
using Nat20Server.Entities;

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
        public async Task<Campaign?> GetCampaignByIdAsync(string id)
        {
            return await _campaignsCollection.Find(campaign => campaign.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new campaign in the database.
        /// </summary>
        /// <param name="campaign">The campaign to create.</param>
        /// <param name="ownerId">The ID of the user who owns the campaign.</param>
        public async Task<Campaign> CreateCampaignAsync(CampaignDto campaign, string ownerId)
        {
            var campaignEntity = new Campaign
            {
                Name = campaign.Name,
                Description = campaign.Description,
                Setting = campaign.Setting,
                OwnerId = ownerId
            };
            await _campaignsCollection.InsertOneAsync(campaignEntity);
            return campaignEntity;
        }
    }
}
