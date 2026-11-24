using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nat20Server.Auth;
using Nat20Server.DTOs;
using Nat20Server.Services;

namespace Nat20Server.Controllers
{
    /// <summary>
    /// REST Controller for Campaign management. All endpoints require authentication.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/campaign")]
    public class CampaignController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly CampaignService _campaignService;

        private readonly CharacterService _characterService;

        public CampaignController(UserService userService, CampaignService campaignService, CharacterService characterService)
        {
            _userService = userService;
            _campaignService = campaignService;
            _characterService = characterService;
        }

        /// <summary>
        /// Retrieves a campaign by id. Only the owner or a player in the campaign can see it.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignById(string id)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(id);

            // Return 404 (not 403) for campaigns the caller can't access, so ids can't be probed.
            if (campaign == null
                || (campaign.OwnerId != userId && !campaign.PlayerIds.Contains(userId)))
            {
                return NotFound();
            }
            return Ok(campaign);
        }

        /// <summary>
        /// Retrieves all campaigns owned by or shared with the user.
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCampaignsById()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var campaigns = await _campaignService.GetAllCampaignsByIdAsync(userId);
            return Ok(campaigns);
        }

        /// <summary>
        /// Creates a new campaign owned by the authenticated user.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCampaignAsync([FromBody] CampaignDto campaignDto)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var campaign = await _campaignService.CreateCampaignAsync(campaignDto, userId);
            return CreatedAtAction(nameof(GetCampaignById), new { id = campaign.Id }, campaign);
        }

        [HttpPost("share")]
        public async Task<IActionResult> ShareCampaign(string campaignId, [FromBody] string userEmail)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(campaignId);
            if (campaign == null || campaign.OwnerId != userId)
            {
                return NotFound();
            }

            var targetUser = await _userService.GetUserByEmailAsync(userEmail);
            if (targetUser == null)
            {
                return BadRequest("User not found.");
            }

            await _campaignService.ShareCampaign(campaign, targetUser.Id);

            return Ok();
        }

        [HttpGet("{campaignId}/active-session/roster")]
        public async Task<IActionResult> GetPartyRoster(string campaignId)
        {
            var userId = User.GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            if (!MongoDB.Bson.ObjectId.TryParse(campaignId, out _))
            {
                return BadRequest("Invalid campaign ID.");
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(campaignId);

            if (campaign is null ||
                (campaign.OwnerId != userId &&
                 !campaign.PlayerIds.Contains(userId)))
            {
                return NotFound();
            }

            var session = campaign.ActiveSession;

            if (session is null)
            {
                return Ok(new PartyRosterDto(null, []));
            }

            var members = session.Members
                .Where(member => member.IsAllied && member.IsParticipating)
                .OrderBy(member => member.Name, StringComparer.OrdinalIgnoreCase)
                .Select(member => new PartyMemberDto(
                    member.Id,
                    member.Name,
                    member.CharacterClass,
                    member.Level))
                .ToArray();

            return Ok(new PartyRosterDto(
                new SessionSummaryDto(session.Id, session.Name),
                members));
        }

        [HttpPost("{campaignId}/characters/{characterId}")]
        public async Task<IActionResult> AddCharacterToCampaign(string campaignId, string characterId)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!MongoDB.Bson.ObjectId.TryParse(campaignId, out _) || !MongoDB.Bson.ObjectId.TryParse(characterId, out _))
            {
                return BadRequest("Invalid ID format.");
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(campaignId);
            if (campaign == null || (campaign.OwnerId != userId && !campaign.PlayerIds.Contains(userId)))
            {
                return NotFound();
            }

            if (campaign.Status != Entities.CampaignStatus.Active)
            {
                return BadRequest("Characters can only be added to active campaigns.");
            }

            var character = await _characterService.GetCharacterByIdAsync(characterId);
            if (character == null || character.OwnerId != userId)
            {
                return Forbid();
            }

            await _campaignService.AddCharacterToCampaignAsync(campaignId, characterId);
            return Ok(character);
        }

        [HttpGet("{campaignId}/characters")]
        public async Task<IActionResult> GetCampaignCharacters(string campaignId)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!MongoDB.Bson.ObjectId.TryParse(campaignId, out _))
            {
                return BadRequest("Invalid campaign ID.");
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(campaignId);
            if (campaign == null || (campaign.OwnerId != userId && !campaign.PlayerIds.Contains(userId)))
            {
                return NotFound();
            }

            var characters = await _characterService.GetCharactersByIdsAsync(campaign.CharacterIds);
            return Ok(characters);
        }

        [HttpGet("{campaignId}/characters/{characterId}")]
        public async Task<IActionResult> GetCampaignCharacterById(string campaignId, string characterId)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!MongoDB.Bson.ObjectId.TryParse(campaignId, out _) || !MongoDB.Bson.ObjectId.TryParse(characterId, out _))
            {
                return BadRequest("Invalid ID format.");
            }

            var campaign = await _campaignService.GetCampaignByIdAsync(campaignId);
            if (campaign == null || (campaign.OwnerId != userId && !campaign.PlayerIds.Contains(userId)))
            {
                return NotFound();
            }

            if (!campaign.CharacterIds.Contains(characterId))
            {
                return NotFound();
            }

            var character = await _characterService.GetCharacterByIdAsync(characterId);
            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }

    }
}
