using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Data;
using WebApplicationAPI.Dtos.Common;
using WebApplicationAPI.Dtos.VideoGame;

namespace WebApplicationAPI.Services.VideoGame
{
    public class VideoGameService(AppDbContext context) : IVideoGameService
    {
        public async Task<PagedResult<VideoGameDto>> GetAllVideoGamesAsync(VideoGameQueryParameters query)
        {
            var videoGamesQuery = context.VideoGames.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                videoGamesQuery = videoGamesQuery.Where(vg => vg.Name.Contains(query.Search));
            }

            var totalCount = await videoGamesQuery.CountAsync();

            var page = Math.Max(1, query.Page);
            var pageSize = Math.Max(1, query.PageSize);

            var items = await videoGamesQuery
                .OrderBy(vg => vg.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(vg => new VideoGameDto
                {
                    Id = vg.Id,
                    Name = vg.Name,
                    Genre = vg.Genre,
                    ReleaseDate = vg.ReleaseDate
                })
                .ToListAsync();

            return new PagedResult<VideoGameDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<VideoGameDto?> GetVideoGameByIdAsync(int id)
        {
            var videoGame = await context.VideoGames.FindAsync(id);
            if (videoGame is null)
            {
                return null;
            }
            return new VideoGameDto
            {
                Id = videoGame.Id,
                Name = videoGame.Name,
                Genre = videoGame.Genre,
                ReleaseDate = videoGame.ReleaseDate
            };
        }

        public async Task<VideoGameDto?> ReleaseVideoGameAsync(int id)
        {
            var videoGame = await context.VideoGames.FindAsync(id);
            if (videoGame is null)
            {
                return null;
            }
            if (videoGame.ReleaseDate != null) {
                throw new ArgumentException("Game is already released!");
            }
            videoGame.ReleaseDate = DateTime.UtcNow;

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
