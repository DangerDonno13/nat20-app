using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nat20Server.Entities
{
    public class Party
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CampaignId { get; set; } = null!;
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> CharacterIds { get; set; } = new List<string>();
    }
}
