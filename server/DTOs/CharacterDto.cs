namespace Nat20Server.DTOs
{
    public class CharacterDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Race { get; set; }
        public string? Class { get; set; }
        public string? Background { get; set; }
        public List<string> Inventory { get; set; } = new List<string>();
        public List<string>? Spells { get; set; }
        public List<string>? Skills { get; set; }
    }
}
