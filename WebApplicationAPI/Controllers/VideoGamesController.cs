using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Dtos.Common;
using WebApplicationAPI.Dtos.VideoGame;
using WebApplicationAPI.Services.VideoGame;

namespace WebApplicationAPI.Controllers
{
    [Route("api/videoGames")]
    [ApiController]
    public class VideoGamesController(IVideoGameService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<VideoGameDto>>> GetAllVideoGames([FromQuery] VideoGameQueryParameters query)
        {
            var videoGames = await service.GetAllVideoGamesAsync(query);
            return Ok(videoGames);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGameDto>> GetVideoGameById(int id)
        {
            var videoGame = await service.GetVideoGameByIdAsync(id);
            if (videoGame is null)
            {
                return NotFound("Video game not found");
            }
            return Ok(videoGame);
        }

        [HttpPost]
        public async Task<ActionResult<VideoGameDto>> CreateVideoGame(CreateUpdateVideoGameDto videoGameInfo)
        {
            var createdVideoGame = await service.CreateVideoGameAsync(videoGameInfo);

            return Ok(createdVideoGame);
        }

        [HttpPut("release/{id}")]
        public async Task<ActionResult<VideoGameDto>> ReleaseVideoGame(int id)
        {
            try
            {
                var releasedVideoGame = await service.ReleaseVideoGameAsync(id);
                if (releasedVideoGame is null)
                {
                    return NotFound("Video game not found");
                }

                return Ok(releasedVideoGame);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

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
