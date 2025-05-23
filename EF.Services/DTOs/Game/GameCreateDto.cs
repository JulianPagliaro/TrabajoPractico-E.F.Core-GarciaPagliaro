namespace EF.Services.DTOs.Game
{
    public class GameCreateDto
    {
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public DateOnly PublishDate { get; set; }
        public decimal Price { get; set; }
        public int DeveloperId { get; set; }
    }
}
