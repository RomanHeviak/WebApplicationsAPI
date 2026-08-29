using MongoDB.Bson;
using MongoDB.Driver;
using WebApplicationAPI.Data;
using WebApplicationAPI.Dtos.Common;
using WebApplicationAPI.Dtos.VideoGame;

namespace WebApplicationAPI.Services.VideoGame
{
    public class VideoGameService(MongoDbContext context) : IVideoGameService
    {
        public async Task<PagedResult<VideoGameDto>> GetAllVideoGamesAsync(VideoGameQueryParameters query)
        {
            var filter = string.IsNullOrWhiteSpace(query.Search)
                ? Builders<Models.VideoGame>.Filter.Empty
                : Builders<Models.VideoGame>.Filter.Regex(
                    vg => vg.Name, new BsonRegularExpression(query.Search, "i"));

            var totalCount = await context.VideoGames.CountDocumentsAsync(filter);

            var page = Math.Max(1, query.Page);
            var pageSize = Math.Max(1, query.PageSize);

            var videoGames = await context.VideoGames.Find(filter)
                .SortBy(vg => vg.Id)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var items = videoGames.Select(vg => new VideoGameDto
            {
                Id = vg.Id,
                Name = vg.Name,
                Genre = vg.Genre,
                ReleaseDate = vg.ReleaseDate
            }).ToList();

            return new PagedResult<VideoGameDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = (int)totalCount
            };
        }

        public async Task<VideoGameDto?> GetVideoGameByIdAsync(string id)
        {
            var videoGame = await context.VideoGames
                .Find(vg => vg.Id == id).FirstOrDefaultAsync();
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

        public async Task<VideoGameDto?> ReleaseVideoGameAsync(string id)
        {
            var videoGame = await context.VideoGames
                .Find(vg => vg.Id == id).FirstOrDefaultAsync();
            if (videoGame is null)
            {
                return null;
            }
            if (videoGame.ReleaseDate != null) {
                throw new ArgumentException("Game is already released!");
            }
            videoGame.ReleaseDate = DateTime.UtcNow;

            await context.VideoGames.ReplaceOneAsync(vg => vg.Id == id, videoGame);

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

            await context.VideoGames.InsertOneAsync(newVideoGame);

            return new VideoGameDto
            {
                Id = newVideoGame.Id,
                Name = newVideoGame.Name,
                Genre = newVideoGame.Genre,
                ReleaseDate = newVideoGame.ReleaseDate
            };
        }

        public async Task<VideoGameDto?> UpdateVideoGameAsync(string id, CreateUpdateVideoGameDto videoGameInfo)
        {
            var videoGame = await context.VideoGames
                .Find(vg => vg.Id == id).FirstOrDefaultAsync();
            if (videoGame == null)
            {
                return null;
            }

            videoGame.Name = videoGameInfo.Name;
            videoGame.Genre = videoGameInfo.Genre;

            await context.VideoGames.ReplaceOneAsync(vg => vg.Id == id, videoGame);

            return new VideoGameDto
            {
                Id = videoGame.Id,
                Name = videoGame.Name,
                Genre = videoGame.Genre,
                ReleaseDate = videoGame.ReleaseDate
            };
        }

        public async Task<bool> DeleteVideoGameAsync(string id)
        {
            var result = await context.VideoGames.DeleteOneAsync(vg => vg.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
