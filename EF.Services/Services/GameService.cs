using EF.Data;
using EF.Entities;
using EF.Services.DTOs.Developer;
using EF.Services.DTOs.Game;
using EF.Services.Interfaces;
using EF.Services.Mappers;
using EF.Services.Validators;

namespace EF.Services.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork = null!;
        public GameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Create(GameCreateDto gameDto, out List<string> errors)
        {
            errors = new List<string>();
            Game game = GameMapper.ToEntity(gameDto);
            if (_unitOfWork.Games.Exist(game.Title, game.DeveloperId))
            {
                errors.Add("Game already exist");
                return false;
            }
            GameValidator gameValidator = new GameValidator();
            if (!UniversalValidator.IsValid(game, gameValidator, out errors))
            {
                return false;
            }
            _unitOfWork.Games.Add(game);
            _unitOfWork.Complete();
            return true;
        }

        public bool Delete(int gameId, out List<string> errors)
        {
            errors = new List<string>();
            if (_unitOfWork.Games.GetById(gameId) is null)
            {
                errors.Add("Game ID not found");
                return false;
            }
            _unitOfWork.Games.Delete(gameId);
            _unitOfWork.Complete();
            return true;
        }

        public bool Exist(string title, int gameDeveloperId, int? excludeId = null)
        {
            return _unitOfWork.Games.Exist(title, gameDeveloperId, excludeId);
        }

        public List<GameWithDeveloperDto> GamesGroupByDeveloper()
        {
            var gamesWithDevelopers = _unitOfWork.Games.GetAll();
            var groupedGames = gamesWithDevelopers
                .GroupBy(g => g.Developer)
                .Select(g => new GameWithDeveloperDto
                {
                    Developer = new DeveloperDto
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Country = g.Key.Country,
                    },
                    Games = g.Select(GameMapper.ToDto).ToList() // Changed from ToBookListDto to ToDto
                }).ToList();
            return groupedGames;
        }

        public List<GameListDto> GetAll(string sortedBy = "Title")
        {
            var games = _unitOfWork.Games.GetAll(sortedBy);
            return games.Select(GameMapper.ToGameListDto).ToList();

        }

        public GameDto? GetById(int gameId)
        {
            var games = _unitOfWork.Games.GetById(gameId);
            return games == null ? null : GameMapper.ToDto(games);

        }



        public bool Update(GameUpdateDto gameDto, out List<string> errors)
        {
            errors = new List<string>();
            Game game = GameMapper.ToEntity(gameDto);
            if (_unitOfWork.Games.Exist(game.Title, game.DeveloperId, game.Id))
            {
                errors.Add("Game already exist");
                return false;
            }
            GameValidator gameValidator = new GameValidator();
            if (!UniversalValidator.IsValid(game, gameValidator, out errors))
            {
                return false;
            }
            _unitOfWork.Games.Update(game);
            _unitOfWork.Complete();
            return true;
        }
    }
}
