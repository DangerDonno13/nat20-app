using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nat20Server.Auth;
using Nat20Server.DTOs;
using Nat20Server.Mappings;
using Nat20Server.Services;

namespace Nat20Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/party")]
    public class PartyController : ControllerBase
    {
        private readonly PartyService _partyService;

        public PartyController(PartyService partyService)
        {
            _partyService = partyService;
        }

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
            return Ok(party.ToDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateParty([FromBody] PartyDto partyDto)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var party = await _partyService.CreatePartyAsync(partyDto);
            return CreatedAtAction(nameof(GetPartyById), new { id = party.Id }, party.ToDto());
        }

        [HttpPost("{partyId}/add-character")]
        public async Task<IActionResult> AddCharacter(string partyId, [FromBody] AddCharacterRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var updatedParty = await _partyService.AddCharacterAsync(partyId, request.CharacterId);
            if (updatedParty == null)
            {
                return NotFound();
            }

            return Ok(updatedParty.ToDto());
        }
    }
}
