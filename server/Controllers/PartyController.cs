using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nat20Server.Auth;
using Nat20Server.DTOs.Parties;
using Nat20Server.Services;

namespace Nat20Server.Controllers
{
    /// <summary>
    /// REST controller for Party management. All endpoints require authentication.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/party")]
    public class PartyController : ControllerBase
    {
        private readonly PartyService _partyService;
        private readonly CharacterService _characterService;
        private readonly CampaignService _campaignService;

        public PartyController(PartyService partyService, CharacterService characterService, CampaignService campaignService)
        {
            _partyService = partyService;
            _characterService = characterService;
            _campaignService = campaignService;
        }

        /// <summary>
        /// Retrieves a party by id. Only the owner of the campaign can see it.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartyById(string id)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var party = await _partyService.GetPartyByIdAsync(id);
            if (party == null)
            {
                return NotFound();
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(party.CampaignId);
            if (campaign == null)
            {
                return NotFound();
            }
            if (campaign.OwnerId != userId)
            {
                return Forbid();
            }


            return Ok(party);
        }

        /// <summary>
        /// Creates a new party in a campaign. Only the owner of the campaign can create parties.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateParty([FromBody] CreatePartyRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(request.CampaignId);
            if (campaign == null)
            {
                return NotFound();
            }
            if (campaign.OwnerId != userId)
            {
                return Forbid();
            }

            var party = await _partyService.CreatePartyAsync(request);
            return CreatedAtAction(nameof(GetPartyById), new { id = party.Id }, party);
        }

        /// <summary>
        /// Adds a character to a party. Only the owner of the campaign can add a character to a party.
        /// </summary>
        /// <param name="partyId">The ID of the party to add the character to.</param>
        /// <param name="request">The character to add.</param>
        /// <returns>A success response containing the updated party object when successful.</returns>
        [HttpPost("{partyId}/add-character")]
        public async Task<IActionResult> AddCharacter(string partyId, [FromBody] AddCharacterRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var party = await _partyService.GetPartyByIdAsync(partyId);
            if (party == null)
            {
                return NotFound();
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(party.CampaignId);
            if (campaign == null)
            {
                return NotFound();
            }

            if (campaign.OwnerId != userId)
            {
                return Forbid();
            }

            var character = await _characterService.GetCharacterByIdAsync(request.CharacterId);
            if (character == null)
            {
                return NotFound();
            }

            var isInCampaign = character.OwnerId == campaign.OwnerId || campaign.PlayerIds.Contains(character.OwnerId);
            if (!isInCampaign)
            {
                return BadRequest("Character does not belong to a member of this campaign.");
            }

            var updatedParty = await _partyService.AddCharacterAsync(partyId, request.CharacterId);
            if (updatedParty == null)
            {
                return NotFound();
            }

            return Ok(updatedParty);
        }
    }
}
