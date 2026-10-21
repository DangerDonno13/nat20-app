using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nat20Server.Entities
{
    public enum CampaignStatus
    {
        Active,
        Inactive,
        Completed
    }

    // This class represents a campaign entity in the application.
    public class Campaign
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Setting { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> PlayerIds { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.String)]
        public CampaignStatus Status { get; set; } = CampaignStatus.Active;
        public string InviteCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
