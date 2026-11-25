namespace Nat20Server.DTOs.Parties
{
    public class PartyResponse
    {
        public string? Id { get; set; }
        public string CampaignId { get; set; } = null!;
        public List<string> CharacterIds { get; set; } = new List<string>();
    }
}
