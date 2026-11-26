using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nat20Server.Auth;
using Nat20Server.DTOs.Characters;
using Nat20Server.Services;

namespace Nat20Server.Controllers
{
    /// <summary>
    /// REST Controller for Character management. All endpoints require authentication.
    /// </summary>
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

        /// <summary>
        /// Retrieves a character by id. Only the owner of the character can see it.
        /// </summary>
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

        /// <summary>
        /// Creates a new character owned by the authenticated user.
        /// </summary>
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

        /// <summary>
        /// Updates a character owned by the authenticated user.
        /// </summary>
        /// <param name="id">The ID of the character to update.</param>
        /// <param name="request">The information to update.</param>
        /// <returns>A success response if the update was successful.</returns>
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
