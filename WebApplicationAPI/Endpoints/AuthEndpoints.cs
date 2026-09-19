using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Dtos.User;
using WebApplicationAPI.Services.Auth;

namespace WebApplicationAPI.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/auth")
                .WithTags("Auth");

            group.MapPost("register", async (CreateUserDto userData, IAuthService service) =>
            {
                try
                {
                    var user = await service.RegisterUserAsync(userData);
                    return Results.Ok(user);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithSummary("🚀 Register a new user")
            .WithDescription("Creates a new user account from the provided details and returns the created user.")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest);

            group.MapPost("login", async (LoginUserDto userData, IAuthService service) =>
            {
                try
                {
                    var user = await service.LoginUserAsync(userData);
                    return Results.Ok(user);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithSummary("🔑 Login a user")
            .WithDescription("Authenticates a user with the provided login credentials and returns the authenticated user.")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest);

            return app;
        }
    }
}
