using EF.Entities;
using EF.Services.DTOs.Game;

namespace EF.Services.Mappers
{
    public static class GameMapper
    {
        public static GameDto ToDto(Game game) => new()
        {
            Id = game.Id,
            Title = game.Title,
            Genre = game.Genre,
            Price = game.Price
        };
        public static GameListDto ToGameListDto(Game game) => new()
        {
            Id = game.Id,
            Title = game.Title
        };
        public static Game ToEntity(GameCreateDto gameDto) => new()
        {
            Title = gameDto.Title,
            Price = gameDto.Price,
            DeveloperId = gameDto.DeveloperId,
            Genre = gameDto.Genre,
            PublishDate = gameDto.PublishDate

        };
        public static Game ToEntity(GameUpdateDto gameDto) => new()
        {
            Id=gameDto.Id,
            Title = gameDto.Title,
            Price = gameDto.Price,
            DeveloperId = gameDto.DeveloperId,
            Genre = gameDto.Genre,
            PublishDate = gameDto.PublishDate

        };
    }
}
