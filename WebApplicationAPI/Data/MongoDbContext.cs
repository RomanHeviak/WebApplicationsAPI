using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Character> Characters => _database.GetCollection<Character>("Characters");
        public IMongoCollection<VideoGame> VideoGames => _database.GetCollection<VideoGame>("VideoGames");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
