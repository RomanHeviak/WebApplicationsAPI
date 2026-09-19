using WebApplicationAPI.Dtos.Common;
using WebApplicationAPI.Dtos.VideoGame;
using WebApplicationAPI.Services.VideoGame;

namespace WebApplicationAPI.Endpoints
{
    public static class VideoGameEndpoints
    {
        public static IEndpointRouteBuilder MapVideoGameEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/videoGames")
                .WithTags("VideoGames");

            group.MapGet("", async ([AsParameters] VideoGameQueryParameters query, IVideoGameService service) =>
            {
                var videoGames = await service.GetAllVideoGamesAsync(query);
                return Results.Ok(videoGames);
            })
            .WithSummary("📋 Get all video games")
            .WithDescription("Retrieves a paged list of video games matching the provided query parameters.")
            .Produces<PagedResult<VideoGameDto>>(StatusCodes.Status200OK);

            group.MapGet("{id}", async (string id, IVideoGameService service) =>
            {
                var videoGame = await service.GetVideoGameByIdAsync(id);
                if (videoGame is null)
                {
                    return Results.NotFound("Video game not found");
                }
                return Results.Ok(videoGame);
            })
            .WithSummary("🔍 Get a video game by ID")
            .WithDescription("Retrieves a single video game matching the specified ID.")
            .Produces<VideoGameDto>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound);

            group.MapPost("", async (CreateUpdateVideoGameDto videoGameInfo, IVideoGameService service) =>
            {
                var createdVideoGame = await service.CreateVideoGameAsync(videoGameInfo);
                return Results.Ok(createdVideoGame);
            })
            .WithSummary("🚀 Create a new video game")
            .WithDescription("Creates a new video game with the provided information.")
            .Produces<VideoGameDto>(StatusCodes.Status200OK);

            group.MapPut("release/{id}", async (string id, IVideoGameService service) =>
            {
                try
                {
                    var releasedVideoGame = await service.ReleaseVideoGameAsync(id);
                    if (releasedVideoGame is null)
                    {
                        return Results.NotFound("Video game not found");
                    }

                    return Results.Ok(releasedVideoGame);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithSummary("🚀 Release a video game")
            .WithDescription("Marks the video game matching the specified ID as released.")
            .Produces<VideoGameDto>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status400BadRequest);

            group.MapPut("{id}", async (string id, CreateUpdateVideoGameDto videoGameInfo, IVideoGameService service) =>
            {
                var updatedVideoGame = await service.UpdateVideoGameAsync(id, videoGameInfo);

                if (updatedVideoGame is null)
                {
                    return Results.NotFound("Video game not found");
                }

                return Results.Ok(updatedVideoGame);
            })
            .WithSummary("🚀 Update an existing video game")
            .WithDescription("Updates the video game matching the specified ID with the provided information.")
            .Produces<VideoGameDto>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound);

            group.MapDelete("{id}", async (string id, IVideoGameService service) =>
            {
                var isDeleted = await service.DeleteVideoGameAsync(id);
                if (!isDeleted)
                {
                    return Results.NotFound("Video game not found");
                }
                return Results.Ok(isDeleted);
            })
            .WithSummary("💀 Delete a video game")
            .WithDescription("Deletes the video game matching the specified ID.")
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
