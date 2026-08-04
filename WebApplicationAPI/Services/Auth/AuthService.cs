using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApplicationAPI.Data;
using WebApplicationAPI.Dtos.User;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Services.Auth
{
    public class AuthService(AppDbContext context) : IAuthService
    {
        public async Task<UserDto?> RegisterUser(CreateUserDto userData)
        {
            var firstName = userData.FirstName.Trim();
            var lastName = userData.LastName.Trim();

            var password = DecodePassword(userData.Password);

            var userExists = await context.Users
                .AnyAsync(u => u.FirstName == firstName && u.LastName == lastName);

            if (userExists)
            {
                throw new InvalidOperationException($"A user with the name '{firstName} {lastName}' already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName
            };

            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, password);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        private static string DecodePassword(string base64Password)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64Password);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("The password is not a valid Base64-encoded string.");
            }
        }
    }
}
