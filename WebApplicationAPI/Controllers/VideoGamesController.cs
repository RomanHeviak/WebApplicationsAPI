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
    }
}
