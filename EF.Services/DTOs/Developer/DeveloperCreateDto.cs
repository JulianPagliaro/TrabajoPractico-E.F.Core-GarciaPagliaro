namespace EF.Services.DTOs.Developer
{
    public class DeveloperCreateDto
    {
        public string Name { get; set; } = null!;
        public DateOnly FoundationDate { get; set; }
        public string Country { get; set; } = null!;

    }
}
