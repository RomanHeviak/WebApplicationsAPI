using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplicationAPI.Models
{
    public class VideoGame
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
    }
}
