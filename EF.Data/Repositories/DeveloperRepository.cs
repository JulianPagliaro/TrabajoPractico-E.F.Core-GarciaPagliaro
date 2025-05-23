using EF.Data.Interfaces;
using EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Data.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly GameDeveloperContext _context = null!;

        public DeveloperRepository(GameDeveloperContext context)
        {
            _context = context;
        }
        public List<Developer> GetAll(string sortedBy = "Name")
        {
            IQueryable<Developer> query = _context.Developers.AsNoTracking();
            //switch (sortedBy)
            //{
            //    case "Name":
            //        return query.OrderBy(d => d.Name)
            //            .ToList();
            //    case "Country":
            //        return query.OrderBy(d => d.Country)
            //            .ToList();
            //    default:
            //        return query.OrderBy(d => d.Id)
            //            .ToList();
            //        break;
            //}

            //FORMA BANANA jaja
            return sortedBy switch
            {
                "Name" => query.OrderBy(d => d.Name)
                                .ThenBy(d=>d.Country)
                                .ToList(),
                "Country" => query.OrderBy(d => d.Country)
                                    .ThenBy(d=>d.Name)
                                    .ToList(),
                _ => query.OrderBy(d => d.Id).ToList()
            };
        }

        public Developer? GetById(int developerId, bool tracked = false)
        {
            //IQueryable<Developer>? query = _context.Developers.FirstOrDefault
            //    (d => d.Id == developerId) as IQueryable<Developer>;
            return tracked
                      ? _context.Developers
                         .FirstOrDefault(d => d.Id == developerId)
                      : _context.Developers
                         .AsNoTracking()
                         .FirstOrDefault(d => d.Id == developerId);
        }

     
        public void Add(Developer developer)
        {
            _context.Developers.Add(developer);
        }
        public void Delete(int developerId)
        {
            var developerInDb = GetById(developerId,true);
            if (developerInDb != null)
            {
                _context.Developers.Remove(developerInDb);
                _context.SaveChanges();
            }
        }
       

        public bool HasDependencies(int developerId)
        {
            return _context.Games.Any(g => g.DeveloperId == developerId);
        }
        public void LoadGames(Developer developer)
        {
            _context.Entry(developer).Collection(d => d.Games).Load();
        }

        public List<Developer> GetAllWithGames()
        {
            return _context.Developers.Include(d=>d.Games).ToList();
        }


        public Developer? GetByName(string Name)
        {
            return _context.Developers.FirstOrDefault(d => d.Name== Name);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool Exist(string name, string Country, DateOnly FoundationDate, int? excludeId = null)
        {
            return excludeId.HasValue
               ? _context.Developers.Any
               (d => d.Name == name 
               && d.Country == Country
               && d.FoundationDate == FoundationDate
               && d.Id != excludeId.Value)
               : _context.Developers.Any(d => d.Name == name);
        }

        public void Update(Developer developer)
        {
            var developerInDb = GetById(developer.Id, true);
            if (developerInDb != null)
            {
                developerInDb.Name = developer.Name;
                developerInDb.FoundationDate = developer.FoundationDate;
                developerInDb.Country = developer.Country;
                _context.SaveChanges();
            }
        }
    }
}
