using EF.Services.DTOs.Game;

namespace EF.Services.Interfaces
{
    public interface IGameService
    {
        bool Create(GameCreateDto gameDto, out List<string> errors);
        bool Update(GameUpdateDto gameDto, out List<string> errors);
        bool Delete(int gameId, out List<string> errors);
        bool Exist(string title, int gameDeveloperId, int? excludeId = null);
        List<GameListDto> GetAll(string sortedBy = "Title");
        GameDto? GetById(int gameId);
        List<GameWithDeveloperDto> GamesGroupByDeveloper();

    }
}
