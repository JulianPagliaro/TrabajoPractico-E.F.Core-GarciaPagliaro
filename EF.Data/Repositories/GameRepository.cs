using EF.Data.Interfaces;
using EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EF.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDeveloperContext _context = null!;
        public GameRepository(GameDeveloperContext context)
        {
            _context = context;
        }

        public void Add(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public void Delete(int gameId)
        {
            var gameInDb = GetById(gameId, true);
            if (gameInDb != null)
            {
                _context.Games.Remove(gameInDb);
                _context.SaveChanges();
            }
        }

        


        public bool Exist(string title, int gameDeveloperId, int? excludeId = null)
        {
            return excludeId.HasValue
               ? _context.Games.Any
               (g => g.Title == title
               && g.Id != excludeId.Value)
               : _context.Games.Any(g => g.Title == title);
        }

        public List<IGrouping<int, Game>> GamesGroupByDeveloper()
        {
            return _context.Games.GroupBy(g => g.DeveloperId).ToList();
        }

        public List<Game> GetAll(string sortedBy = "Title", bool include = false)
        {
            IQueryable<Game> query = _context.Games.AsNoTracking();

            return sortedBy switch
            {
                "Title" => query.OrderBy(g => g.Title)
                                .ThenBy(d => d.PublishDate)
                                .ToList(),
                "PublishDate" => query.OrderBy(g => g.PublishDate)
                                    .ThenBy(g => g.Title)
                                    .ToList(),
                _ => query.OrderBy(g => g.Id).ToList()
            };
        }

        public Game? GetById(int gameId, bool tracked = false)
        {
            return tracked
                      ? _context.Games
                         .FirstOrDefault(g => g.Id == gameId)
                      : _context.Games
                         .AsNoTracking()
                         .FirstOrDefault(g => g.Id == gameId);
        }

        public void Update(Game game)
        {
            var gameInDb = GetById(game.Id, true);
            if (gameInDb != null)
            {
                gameInDb.Title = game.Title;
                gameInDb.PublishDate = game.PublishDate;
                gameInDb.Price = game.Price;
                _context.SaveChanges();
            }
        }
    }
}
