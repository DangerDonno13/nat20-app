using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nat20Server.Entities
{
    public sealed class ActiveSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public List<SessionMember> Members { get; set; } = [];
    }
}
