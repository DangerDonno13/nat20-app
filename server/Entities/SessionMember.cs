using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nat20Server.Entities
{
    public sealed class SessionMember
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CharacterClass { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public bool IsAllied { get; set; } = false;
        public bool IsParticipating { get; set; } = false;
    }
}
