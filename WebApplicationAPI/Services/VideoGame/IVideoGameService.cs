using WebApplicationAPI.Dtos.Common;
using WebApplicationAPI.Dtos.VideoGame;

namespace WebApplicationAPI.Services.VideoGame
{
    public interface IVideoGameService
    {
        Task<PagedResult<VideoGameDto>> GetAllVideoGamesAsync(VideoGameQueryParameters query);
        Task<VideoGameDto?> GetVideoGameByIdAsync(int id);
        Task<VideoGameDto> CreateVideoGameAsync(CreateUpdateVideoGameDto videoGame);
        Task<VideoGameDto> ReleaseVideoGameAsync(int id);
        Task<VideoGameDto?> UpdateVideoGameAsync(int id, CreateUpdateVideoGameDto videoGame);
        Task<bool> DeleteVideoGameAsync(int id);
    }
}
