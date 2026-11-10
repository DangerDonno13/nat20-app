using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nat20Server.Entities
{
    public enum CampaignStatus
    {
        Unspecified = 0,
        Active,
        Inactive,
        Completed
    }

    public enum CampaignSetting
    {
        Unspecified = 0,
        ForgottenRealms,
        Eberron,
        Homebrew,

    }

    // This class represents a campaign entity in the application.
    public class Campaign
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public ActiveSession? ActiveSession { get; set; } = null;

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CampaignSetting Setting { get; set; } = CampaignSetting.Unspecified;

        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> PlayerIds { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.String)]
        public CampaignStatus Status { get; set; } = CampaignStatus.Unspecified;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
