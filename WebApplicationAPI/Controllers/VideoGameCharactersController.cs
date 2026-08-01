using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Models;
using WebApplicationAPI.Services.VideoGameCharacter;
using WebApplicationAPI.Dtos.Character;

namespace WebApplicationAPI.Controllers
{
    [Route("api/videoGameCharacters")]
    [ApiController]
    public class VideoGameCharactersController(IVideoGameCharacterService service) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<CharacterDto>>> GetCharacters()
        {
            return Ok(await service.GetAllCharactersAsync());
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult<CharacterDto>> CreateCharacter(CreateCharacterDto characterInfo)
        {
            var createdCharacter = await service.AddCharacterAsync(characterInfo);

            return CreatedAtAction(nameof(GetCharacter), new { id = createdCharacter.Id }, createdCharacter);
        }

        [HttpPut("{id}")]
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
