using Microsoft.AspNetCore.Mvc;
using Nat20Server.DTOs;
using Nat20Server.Entities;
using Nat20Server.Services;

namespace Nat20Server.Controllers
{
    /// <summary>
    /// REST Controller for Campaign management.
    /// </summary>
    [ApiController]
    [Route("api/campaign")]
    public class CampaignController : ControllerBase
    {
        private readonly CampaignService _campaignService;

        public CampaignController(CampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignById(string id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }
            return Ok(campaign);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaignAsync([FromBody] CampaignDto campaignDto,[FromQuery] string userId)
        {
            var campaign = await _campaignService.CreateCampaignAsync(campaignDto, userId);
            return CreatedAtAction(nameof(GetCampaignById), new { id = campaign.Id }, campaign);
        }
    }
}
