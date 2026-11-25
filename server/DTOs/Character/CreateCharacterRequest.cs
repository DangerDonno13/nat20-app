namespace Nat20Server.DTOs.Character
{
    public class CreateCharacterRequest
    {
        public string Name { get; set; } = null!;
        public string? Race { get; set; }
        public string? Class { get; set; }
        public string? Background { get; set; }
        public List<string> Inventory { get; set; } = new List<string>();
        public List<string>? Spells { get; set; }
        public List<string>? Skills { get; set; }
    }
}
