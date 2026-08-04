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
        public async Task<IActionResult> Register(CreateUserDto userData)
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
    }
}
