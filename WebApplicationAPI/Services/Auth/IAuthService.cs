using WebApplicationAPI.Dtos.User;

namespace WebApplicationAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterUser(CreateUserDto userData);
    }
}
