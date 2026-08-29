namespace WebApplicationAPI.Dtos.VideoGame
{
    public class VideoGameDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
    }
}
