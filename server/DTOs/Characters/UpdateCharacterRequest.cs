namespace Nat20Server.DTOs.Characters
{
    public class UpdateCharacterRequest
    {
        public string? Name { get; set; }
        public string? Race { get; set; }
        public string? Class { get; set; }
        public string? Background { get; set; }
        public List<string>? Inventory { get; set; }
        public List<string>? Spells { get; set; }
        public List<string>? Skills { get; set; }
    }
}
