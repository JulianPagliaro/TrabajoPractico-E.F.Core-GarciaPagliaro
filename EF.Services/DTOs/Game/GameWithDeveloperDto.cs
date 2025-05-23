using EF.Services.DTOs.Developer;

namespace EF.Services.DTOs.Game
{
    public class GameWithDeveloperDto
    {
        public DeveloperDto Developer { get; set; } = null!;
        public List<GameDto>? Games { get; set; }
    }
}
