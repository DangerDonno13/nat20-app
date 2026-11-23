using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nat20Server.Entities
{
    public class Party
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public List<string> CharacterIds { get; set; } = new List<string>();
    }
}
