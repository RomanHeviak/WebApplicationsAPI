using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Dtos.VideoGame;
using WebApplicationAPI.Services.VideoGame;

namespace WebApplicationAPI.Controllers
{
    [Route("api/videoGames")]
    [ApiController]
    public class VideoGamesController(IVideoGameService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<VideoGameDto>> CreateVideoGame(CreateUpdateVideoGameDto videoGameInfo)
        {
            var createdVideoGame = await service.CreateVideoGameAsync(videoGameInfo);

            return Ok(createdVideoGame);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VideoGameDto>> UpdateVideoGame(int id, CreateUpdateVideoGameDto videoGameInfo)
        {
            var updatedVideoGame = await service.UpdateVideoGameAsync(id, videoGameInfo);

            if (updatedVideoGame == null) {
                return NotFound("Video game not found");
            }

            return Ok(updatedVideoGame);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVideoGame(int id)
        {
            var isDeleted = await service.DeleteVideoGameAsync(id);
            if (!isDeleted)
            {
                return NotFound("Video game not found");
            }
            return Ok(isDeleted);
        }
    }
}
