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
        [EndpointSummary("📋 Get all video games")]
        [EndpointDescription("Retrieves a paged list of video games matching the provided query parameters.")]
        public async Task<ActionResult<PagedResult<VideoGameDto>>> GetAllVideoGames([FromQuery] VideoGameQueryParameters query)
        {
            var videoGames = await service.GetAllVideoGamesAsync(query);
            return Ok(videoGames);
        }

        [HttpGet("{id}")]
        [EndpointSummary("🔍 Get a video game by ID")]
        [EndpointDescription("Retrieves a single video game matching the specified ID.")]
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
        [EndpointSummary("🚀 Create a new video game")]
        [EndpointDescription("Creates a new video game with the provided information.")]
        public async Task<ActionResult<VideoGameDto>> CreateVideoGame(CreateUpdateVideoGameDto videoGameInfo)
        {
            var createdVideoGame = await service.CreateVideoGameAsync(videoGameInfo);

            return Ok(createdVideoGame);
        }

        [HttpPut("release/{id}")]
        [EndpointSummary("🚀 Release a video game")]
        [EndpointDescription("Marks the video game matching the specified ID as released.")]
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
        [EndpointSummary("🚀 Update an existing video game")]
        [EndpointDescription("Updates the video game matching the specified ID with the provided information.")]
        public async Task<ActionResult<VideoGameDto>> UpdateVideoGame(int id, CreateUpdateVideoGameDto videoGameInfo)
        {
            var updatedVideoGame = await service.UpdateVideoGameAsync(id, videoGameInfo);

            if (updatedVideoGame == null) {
                return NotFound("Video game not found");
            }

            return Ok(updatedVideoGame);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("💀 Delete a video game")]
        [EndpointDescription("Deletes the video game matching the specified ID.")]
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
