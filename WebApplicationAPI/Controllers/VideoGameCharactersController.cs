using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Models;
using WebApplicationAPI.Services.VideoGameCharacter;
using WebApplicationAPI.Dtos.Character;
using Microsoft.AspNetCore.Authorization;

namespace WebApplicationAPI.Controllers
{
    [Route("api/videoGameCharacters")]
    [ApiController]
    public class VideoGameCharactersController(IVideoGameCharacterService service) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [EndpointSummary("📋 Get all video game characters")]
        [EndpointDescription("Retrieves the complete list of video game characters available in the system.")]
        public async Task<ActionResult<List<CharacterDto>>> GetCharacters()
        {
            return Ok(await service.GetAllCharactersAsync());
        }

        [HttpGet("{id}")]
        [EndpointSummary("🔍 Get a video game character by ID")]
        [EndpointDescription("Retrieves a single video game character matching the specified ID.")]
        public async Task<ActionResult<CharacterDto>> GetCharacter(int id)
        {
            var character = await service.GetCharacterByIdAsync(id);
            if (character is null)
            {
                return NotFound("Character not found");
            }
            return Ok(character);
        }

        [HttpPost]
        [EndpointSummary("🚀 Create a new video game character")]
        [EndpointDescription("Creates a new video game character with the provided information.")]
        public async Task<ActionResult<CharacterDto>> CreateCharacter(CreateCharacterDto characterInfo)
        {
            var createdCharacter = await service.AddCharacterAsync(characterInfo);

            return CreatedAtAction(nameof(GetCharacter), new { id = createdCharacter.Id }, createdCharacter);
        }

        [HttpPut("{id}")]
        [EndpointSummary("🚀 Update an existing video game character")]
        [EndpointDescription("Updates the video game character matching the specified ID with the provided information.")]
        public async Task<ActionResult<CharacterDto>> UpdateCharacter(int id, UpdateCharacterDto characterInfo)
        {
            if (!id.Equals(characterInfo.Id))
            {
                return BadRequest("ID mismatch");
            }

            var updated = await service.UpdateCharacterAsync(characterInfo);
            if (updated is null)
            {
                return NotFound("Character not found");
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("💀 Delete a video game character")]
        [EndpointDescription("Deletes the video game character matching the specified ID.")]
        public async Task<ActionResult<bool>> DeleteCharacter(int id)
        {
            var isDeleted = await service.DeleteCharacterAsync(id);
            if (!isDeleted)
            {
                return NotFound("Character not found");
            }

            return Ok(isDeleted);
        }
    }
}
