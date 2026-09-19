using WebApplicationAPI.Dtos.Character;
using WebApplicationAPI.Services.VideoGameCharacter;

namespace WebApplicationAPI.Endpoints
{
    public static class VideoGameCharacterEndpoints
    {
        public static IEndpointRouteBuilder MapVideoGameCharacterEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/videoGameCharacters")
                .WithTags("VideoGameCharacters")
                .RequireAuthorization();

            group.MapGet("", async (IVideoGameCharacterService service) =>
            {
                return Results.Ok(await service.GetAllCharactersAsync());
            })
            .WithSummary("📋 Get all video game characters")
            .WithDescription("Retrieves the complete list of video game characters available in the system.")
            .Produces<List<CharacterDto>>(StatusCodes.Status200OK);

            group.MapGet("{id}", async (string id, IVideoGameCharacterService service) =>
            {
                var character = await service.GetCharacterByIdAsync(id);
                if (character is null)
                {
                    return Results.NotFound("Character not found");
                }
                return Results.Ok(character);
            })
            .WithName("GetVideoGameCharacterById")
            .WithSummary("🔍 Get a video game character by ID")
            .WithDescription("Retrieves a single video game character matching the specified ID.")
            .Produces<CharacterDto>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound);

            group.MapPost("", async (CreateCharacterDto characterInfo, IVideoGameCharacterService service) =>
            {
                var createdCharacter = await service.AddCharacterAsync(characterInfo);
                return Results.CreatedAtRoute("GetVideoGameCharacterById", new { id = createdCharacter.Id }, createdCharacter);
            })
            .WithSummary("🚀 Create a new video game character")
            .WithDescription("Creates a new video game character with the provided information.")
            .Produces<CharacterDto>(StatusCodes.Status201Created);

            group.MapPut("{id}", async (string id, UpdateCharacterDto characterInfo, IVideoGameCharacterService service) =>
            {
                if (!id.Equals(characterInfo.Id))
                {
                    return Results.BadRequest("ID mismatch");
                }

                var updated = await service.UpdateCharacterAsync(characterInfo);
                if (updated is null)
                {
                    return Results.NotFound("Character not found");
                }

                return Results.Ok(updated);
            })
            .WithSummary("🚀 Update an existing video game character")
            .WithDescription("Updates the video game character matching the specified ID with the provided information.")
            .Produces<CharacterDto>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<string>(StatusCodes.Status404NotFound);

            group.MapDelete("{id}", async (string id, IVideoGameCharacterService service) =>
            {
                var isDeleted = await service.DeleteCharacterAsync(id);
                if (!isDeleted)
                {
                    return Results.NotFound("Character not found");
                }

                return Results.Ok(isDeleted);
            })
            .WithSummary("💀 Delete a video game character")
            .WithDescription("Deletes the video game character matching the specified ID.")
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
