using EF.Services.DTOs.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Services.DTOs.Developer
{
    public class DeveloperGamesCountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int GamesCount { get; set; }
        public List<GameDto> Games { get; set; } = null!;

    }
}
