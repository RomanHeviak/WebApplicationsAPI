using WebApplicationAPI.Models;
using WebApplicationAPI.Data;
using WebApplicationAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationAPI.Services
{
    public class VideoGameCharacterService(AppDbContext context) : IVideoGameCharacterService
    {
        public async Task<List<CharacterDto>> GetAllCharactersAsync()
        {
            return await context.Characters
                .Select(c => new CharacterDto
                {
                    Name = c.Name,
                    Game = c.Game,
                    Role = c.Role,
                    Id = c.Id,
                    Description = $"{c.Name} is a {c.Role} in the game {c.Game}.",
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<CharacterDto?> GetCharacterByIdAsync(int id)
        {
            var character = await context.Characters.FindAsync(id);

            if (character is null)
            {
                return null;
            }

            return new CharacterDto
            {
                Id = character.Id,
                Name = character.Name,
                Game = character.Game,
                Role = character.Role,
                Description = $"{character.Name} is a {character.Role} in the game {character.Game}.",
                CreatedAt = character.CreatedAt,
                UpdatedAt = character.UpdatedAt
            };
        }

        public async Task<CharacterDto> AddCharacterAsync(CreateCharacterDto character)
        {
            var newCharacter = new Character
            {
                Name = character.Name,
                Game = character.Game,
                Role = character.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            context.Characters.Add(newCharacter);
            await context.SaveChangesAsync();

            return new CharacterDto
            {
                Id = newCharacter.Id,
                Name = newCharacter.Name,
                Game = newCharacter.Game,
                Role = newCharacter.Role,
                Description = $"{newCharacter.Name} is a {newCharacter.Role} in the game {newCharacter.Game}.",
                CreatedAt = newCharacter.CreatedAt,
                UpdatedAt= newCharacter.UpdatedAt
            };
        }

        public async Task<CharacterDto?> UpdateCharacterAsync(UpdateCharacterDto character)
        {
            var updateCharacter = await context.Characters.FindAsync(character.Id);
            if (updateCharacter == null) {
                return null;
            }

            updateCharacter.Name = character.Name;
            updateCharacter.Game = character.Game;
            updateCharacter.Role = character.Role;
            updateCharacter.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();


            context.Characters.Update(updateCharacter);
            await context.SaveChangesAsync();

            return new CharacterDto
            {
                Id = updateCharacter.Id,
                Name = updateCharacter.Name,
                Game = updateCharacter.Game,
                Role = updateCharacter.Role,
                Description = $"{updateCharacter.Name} is a {updateCharacter.Role} in the game {updateCharacter.Game}.",
                CreatedAt = updateCharacter.CreatedAt,
                UpdatedAt = updateCharacter.UpdatedAt
            };
        }

        public async Task<bool> DeleteCharacterAsync(int id)
        {
            var character = await context.Characters.FindAsync(id);
            if (character is null)
            {
                return false;
            }

            context.Characters.Remove(character);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
