using EF.Entities;
using EF.Services.DTOs.Developer;

namespace EF.Services.Interfaces
{
    public interface IDeveloperService
    {
        bool Create(DeveloperCreateDto developerDto, out DeveloperDto? developerCreated, out List<string> errors);
        bool Create (DeveloperCreateDto developerDto, out List<string> errors);
        bool Update (DeveloperUpdateDto developerDto, out List<string> errors);
        bool Delete(int developerId, out List<string> errors);
        bool Exist(string name,string country,DateOnly foundationDate, int? excludeId = null);
        List<DeveloperDto> GetAll(string sortedBy = "Name");
        DeveloperDto? GetById(int developerId);
        DeveloperDto? GetByName(string name, string country, DateOnly foundationDate);
        bool hasGames(int developerId);
        void LoadGames(Developer developer);
        List<DeveloperWithGameDto> GetAllWithGames();
        List<DeveloperGamesCountDto> DevelopersGroupIdGames();
    }
}
