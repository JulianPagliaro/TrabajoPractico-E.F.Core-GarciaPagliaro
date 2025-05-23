using EF.Data;
using EF.Data.Interfaces;
using EF.Entities;
using EF.Services.DTOs.Developer;
using EF.Services.DTOs.Game;
using EF.Services.Interfaces;
using EF.Services.Mappers;
using EF.Services.Validators;

namespace EF.Services.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IUnitOfWork _unitOfWork = null!;

        public DeveloperService(IUnitOfWork unityOfWork)
        {
            _unitOfWork = unityOfWork;
        }

        public bool Create(DeveloperCreateDto developerDto, out List<string> errors)
        {
            errors = new List<string>();
            Developer developer = DeveloperMapper.ToEntity(developerDto);
            if (_unitOfWork.Developers.Exist(developer.Name
                , developer.Country
                , developer.FoundationDate))
            {
                errors.Add($"Developer with name {developer.Name} already exists");
                return false;
            }
            var developerValidator = new DevelopersValidator();
            if (!UniversalValidator.IsValid(developer, developerValidator, out errors))
            {
                return false;
            }
            _unitOfWork.Developers.Add(developer);
            _unitOfWork.Complete();
            return true;
        }

        public bool Create(DeveloperCreateDto developerDto, out DeveloperDto? developerCreated, out List<string> errors)
        {
            errors = new List<string>();
            developerCreated = null;
            Developer developer = DeveloperMapper.ToEntity(developerDto);
            if (_unitOfWork.Developers.Exist(developer.Name
                , developer.Country
                , developer.FoundationDate))
            {
                errors.Add($"Developer with name {developer.Name} already exists");
                return false;
            }
            var developerValidator = new DevelopersValidator();
            if (!UniversalValidator.IsValid(developer, developerValidator, out errors))
            {
                return false;
            }
            _unitOfWork.Developers.Add(developer);
            _unitOfWork.Complete();
            developerCreated = DeveloperMapper.ToDto(developer);
            return true;
        }

        public bool Delete(int developerId, out List<string> errors)
        {
            errors = new List<string>();
            if (_unitOfWork.Developers.GetById(developerId) is null)
            {
                errors.Add("Developer does not exist!!");
                return false;
            }
            if (_unitOfWork.Developers.HasDependencies(developerId))
            {
                //TODO: Pedir al otro repo los libros
                errors.Add("Author with dependencies!!!");
                return false;
            }
            _unitOfWork.Developers.Delete(developerId);
            _unitOfWork.Complete();
            return true;
        }

        public List<DeveloperGamesCountDto> DevelopersGroupIdGames()
        {
            var developerWithGames = _unitOfWork.Developers.GetAllWithGames();
            return developerWithGames.Select(d => new DeveloperGamesCountDto
            {
                Id = d.Id,
                Name = $"{d.Name}",
                GamesCount = d.Games is null ? 0 : d.Games.Count,
                Games = d.Games != null
               ? d.Games.Select(GameMapper.ToDto).ToList() :
               new List<GameDto>()
            }).ToList();

        }

        public bool Exist(string name, string country, DateOnly foundationDate, int? excludeId = null)
        {
            return _unitOfWork.Developers.Exist(name, country, foundationDate, excludeId);
        }

        public List<DeveloperDto> GetAll(string sortedBy = "Name")
        {
            var developers = _unitOfWork.Developers.GetAll(sortedBy);
            return developers.Select(DeveloperMapper.ToDto).ToList();
        }

        public List<DeveloperWithGameDto> GetAllWithGames()
        {
            var developerWithGames = _unitOfWork.Developers.GetAllWithGames();
            return developerWithGames.Select(d => new DeveloperWithGameDto
            {
                Id = d.Id,
                Name = $"{d.Name}",
                Games = d.Games != null
                ? d.Games.Select(GameMapper.ToDto).ToList() :
                new List<GameDto>()
            }).ToList();
        }

        public Developer? GetById(int developerId, bool tracked = false)
        {
            return _unitOfWork.Developers.GetById(developerId, tracked);
        }

        public DeveloperDto? GetById(int developerId)
        {
            var developer = _unitOfWork.Developers.GetById(developerId);
            return developer is null ? null : DeveloperMapper.ToDto(developer);
        }

        public DeveloperDto? GetByName(string name, string country, DateOnly foundationDate)
        {
            var developer = _unitOfWork.Developers.GetByName(name);
            return developer is null ? null : DeveloperMapper.ToDto(developer);
        }

        public bool hasGames(int developerId)
        {
            return _unitOfWork.Developers.HasDependencies(developerId);
        }

        public void LoadGames(Developer developer)
        {
            _unitOfWork.Developers.LoadGames(developer);
        }

        public bool Update(DeveloperUpdateDto developerDto, out List<string> errors)
        {
            errors = new List<string>();
            Developer developer = DeveloperMapper.ToEntity(developerDto);
            if (_unitOfWork.Developers.Exist(developerDto.Name,
               developerDto.Country,
               developerDto.FoundationDate))
            {
                errors.Add($"Developer with name {developer.Name} already exists");
                return false;
            }
            var developerValidator = new DevelopersValidator();
            if (!UniversalValidator.IsValid(developer, developerValidator, out errors))
            {
                return false;
            }
            _unitOfWork.Developers.Update(developer);
            _unitOfWork.Complete();
            return true;

        }
    }
}
