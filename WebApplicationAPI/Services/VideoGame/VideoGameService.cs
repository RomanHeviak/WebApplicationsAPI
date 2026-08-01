using WebApplicationAPI.Data;
using WebApplicationAPI.Dtos.Character;
using WebApplicationAPI.Dtos.VideoGame;
using WebApplicationAPI.Models;

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

        public async Task<VideoGameDto?> UpdateVideoGameAsync(int id, CreateUpdateVideoGameDto videoGameInfo)
        {
            var videoGame = await context.VideoGames.FindAsync(id);
            if (videoGame == null)
            {
                return null;
            }

            videoGame.Name = videoGameInfo.Name;
            videoGame.Genre = videoGameInfo.Genre;


            context.VideoGames.Update(videoGame);
            await context.SaveChangesAsync();

            return new VideoGameDto
            {
                Id = videoGame.Id,
                Name = videoGame.Name,
                Genre = videoGame.Genre,
                ReleaseDate = videoGame.ReleaseDate
            };
        }

        public async Task<bool> DeleteVideoGameAsync(int id)
        {
            var videoGame = await context.VideoGames.FindAsync(id);
            if (videoGame is null)
            {
                return false;
            }

            context.VideoGames.Remove(videoGame);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
