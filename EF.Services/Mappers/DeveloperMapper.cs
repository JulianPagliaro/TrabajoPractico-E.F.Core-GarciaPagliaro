using EF.Entities;
using EF.Services.DTOs.Developer;

namespace EF.Services.Mappers
{
    public static class DeveloperMapper
    {
        public static DeveloperDto ToDto(Developer developer) => new()
        {
            Id = developer.Id,
            Name = developer.Name,
            FoundationDate = developer.FoundationDate,
            Country = developer.Country

        };
        public static Developer ToEntity(DeveloperCreateDto developerDto) => new()
        {
            Name = developerDto.Name,
            FoundationDate = developerDto.FoundationDate,
            Country = developerDto.Country
        };
        public static Developer ToEntity(DeveloperUpdateDto developerDto) => new()
        {
            Id = developerDto.Id,
            Name = developerDto.Name,
            FoundationDate = developerDto.FoundationDate,
            Country = developerDto.Country
        };

    }
}
