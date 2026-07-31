using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    {
        public DbSet<Character> Characters { get; set; }
    }
}
