namespace WebApplicationAPI.Dtos
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Game { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
