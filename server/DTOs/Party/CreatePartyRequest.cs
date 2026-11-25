namespace Nat20Server.DTOs.Party
{
    public class CreatePartyRequest
    {
        public string CampaignId { get; set; } = null!;
        public List<string> CharacterIds { get; set; } = new List<string>();
    }
}
