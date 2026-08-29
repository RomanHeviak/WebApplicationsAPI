using WebApplicationAPI.Dtos.Common;
using WebApplicationAPI.Dtos.VideoGame;

namespace WebApplicationAPI.Services.VideoGame
{
    public interface IVideoGameService
    {
        Task<PagedResult<VideoGameDto>> GetAllVideoGamesAsync(VideoGameQueryParameters query);
        Task<VideoGameDto?> GetVideoGameByIdAsync(string id);
        Task<VideoGameDto> CreateVideoGameAsync(CreateUpdateVideoGameDto videoGame);
        Task<VideoGameDto> ReleaseVideoGameAsync(string id);
        Task<VideoGameDto?> UpdateVideoGameAsync(string id, CreateUpdateVideoGameDto videoGame);
        Task<bool> DeleteVideoGameAsync(string id);
    }
}
