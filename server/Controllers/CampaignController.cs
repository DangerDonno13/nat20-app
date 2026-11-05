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

        public CampaignController(UserService userService, CampaignService campaignService)
        {
            _userService = userService;
            _campaignService = campaignService;
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
        public async Task<IActionResult> ShareCampaign(string campaignId,[FromBody] string userEmail)
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
    }
}