using WebApplicationAPI.Dtos.User;

namespace WebApplicationAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto> RegisterUserAsync(CreateUserDto userData);
        Task<UserDto> LoginUserAsync(LoginUserDto userData);
    }
}
