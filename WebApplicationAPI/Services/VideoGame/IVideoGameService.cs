using WebApplicationAPI.Dtos.VideoGame;

namespace WebApplicationAPI.Services.VideoGame
{
    public interface IVideoGameService
    {
        Task<VideoGameDto> CreateVideoGameAsync(CreateUpdateVideoGameDto videoGame);
        Task<VideoGameDto?> UpdateVideoGameAsync(int id, CreateUpdateVideoGameDto videoGame);
        Task<bool> DeleteVideoGameAsync(int id);
    }
}
