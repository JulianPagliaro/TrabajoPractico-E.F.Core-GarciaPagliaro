namespace EF.Services.DTOs.Developer
{
    public class DeveloperUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public DateOnly FoundationDate { get; set; }
    }
}
