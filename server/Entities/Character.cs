using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nat20Server.Entities
{
    public class Character
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; } = null!;


        // Character info
        public string Name { get; set; } = null!;
        public string? Race { get; set; }
        public string? Class { get; set; }
        public string? Background { get; set; }
        public List<string> Inventory { get; set; } = new List<string>();
        public List<string>? Spells { get; set; }
        public List<string>? Skills { get; set; }
    }
}
