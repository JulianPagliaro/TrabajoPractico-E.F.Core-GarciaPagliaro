using EF.Entities;

namespace EF.Data.Interfaces
{
    public interface IDeveloperRepository
    {
        void Add(Developer developer);
        void Delete(int developerId);
       
        bool Exist(string name, string Country, DateOnly FoundationDate, int? excludeId = null);
        List<Developer> GetAll(string sortedBy = "Name");
        Developer? GetById(int developerId, bool tracked = false);
        Developer? GetByName(string Name);

        bool HasDependencies(int developerId);
        void LoadGames(Developer developer);
        List<Developer> GetAllWithGames();

        void SaveChanges();
        void Update(Developer developer);
    }
}