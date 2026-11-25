using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nat20Server.Auth;
using Nat20Server.DTOs.Characters;
using Nat20Server.Services;

namespace Nat20Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/character")]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterService _characterService;

        public CharacterController(CharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterById(string id)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var character = await _characterService.GetCharacterByIdAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            if (character.OwnerId != userId)
            {
                return Forbid();
            }
            return Ok(character);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var character = await _characterService.CreateCharacterAsync(request, userId);
            return CreatedAtAction(nameof(GetCharacterById), new { id = character.Id }, character);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacter(string id, [FromBody] UpdateCharacterRequest request)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var character = await _characterService.GetCharacterByIdAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            if (character.OwnerId != userId)
            {
                return Forbid();
            }
            await _characterService.UpdateCharacterAsync(id, request);
            return NoContent();
        }
    }
}
