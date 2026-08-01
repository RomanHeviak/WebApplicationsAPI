using WebApplicationAPI.Data;
using WebApplicationAPI.Dtos.VideoGame;

namespace WebApplicationAPI.Services.VideoGame
{
    public class VideoGameService(AppDbContext context) : IVideoGameService
    {
        public async Task<VideoGameDto> CreateVideoGameAsync(CreateUpdateVideoGameDto videoGame)
        {
            var newVideoGame = new Models.VideoGame
            {
                Name = videoGame.Name,
                Genre = videoGame.Genre,
                ReleaseDate = null
            };

            context.VideoGames.Add(newVideoGame);
            await context.SaveChangesAsync();

            return new VideoGameDto
            {
                Id = newVideoGame.Id,
                Name = newVideoGame.Name,
                Genre = newVideoGame.Genre,
                ReleaseDate = newVideoGame.ReleaseDate
            };
        }
    }
}
