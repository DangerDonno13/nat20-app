namespace Nat20Server.DTOs
{
    public class PartyDto
    {
        public string? Id { get; set; }
        public List<string> CharacterIds { get; set; } = new List<string>();
    }
}
