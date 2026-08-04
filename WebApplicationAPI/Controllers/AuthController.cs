using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Dtos.User;
using WebApplicationAPI.Services.Auth;

namespace WebApplicationAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register")]
        [EndpointSummary("🚀 Register a new user")]
        [EndpointDescription("Creates a new user account from the provided details and returns the created user.")]
        public async Task<ActionResult<UserDto>> Register(CreateUserDto userData)
        {
            try
            {
                var user = await service.RegisterUser(userData);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [EndpointSummary("🔑 Login a user")]
        [EndpointDescription("Authenticates a user with the provided login credentials and returns the authenticated user.")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto userData)
        {
            try
            {
                var user = await service.LoginUser(userData);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
