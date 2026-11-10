namespace Nat20Server.DTOs
{
    public sealed record PartyRosterDto(SessionSummaryDto? Session, PartyMemberDto[] Members);
}
