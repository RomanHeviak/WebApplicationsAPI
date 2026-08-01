using WebApplicationAPI.Models;
using WebApplicationAPI.Dtos.Character;

namespace WebApplicationAPI.Services.VideoGameCharacter
{
    public interface IVideoGameCharacterService
    {
        Task<List<CharacterDto>> GetAllCharactersAsync();
        Task<CharacterDto?> GetCharacterByIdAsync(int id);
        Task<CharacterDto> AddCharacterAsync(CreateCharacterDto character);
        Task<CharacterDto?> UpdateCharacterAsync(UpdateCharacterDto character);
        Task<bool> DeleteCharacterAsync(int id);
    }
}
