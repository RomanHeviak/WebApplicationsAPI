using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationAPI.Data;
using WebApplicationAPI.Dtos.User;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Services.Auth
{
    public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<UserDto> RegisterUser(CreateUserDto userData)
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
                Login = userData.Login,
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
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = CreateJwtToken(user)
            };
        }

        public async Task<UserDto> LoginUser(LoginUserDto userData)
        {
            var password = DecodePassword(userData.Password);

            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Login == userData.Login)
                ?? throw new InvalidOperationException("Invalid login or password.");

            var result = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid login or password.");
            }

            return new UserDto
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = CreateJwtToken(user)
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

        private string CreateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Login),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresInMinutes = double.TryParse(jwtSettings["ExpiresInMinutes"], out var minutes)
                ? minutes
                : 60;

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
