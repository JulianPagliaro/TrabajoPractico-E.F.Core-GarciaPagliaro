using EF.Entities;

namespace EF.Data.Interfaces
{
    public interface IGameRepository
    {
        void Add(Game game);
        void Delete(int gameId);
        bool Exist(string title, int gameDeveloperId, int? excludeId = null);
        List<Game> GetAll(string sortedBy = "Title", bool include = false);
        Game? GetById(int gameId, bool tracked = false);
        List<IGrouping<int, Game>> GamesGroupByDeveloper();
        void Update(Game game);
    }
}
