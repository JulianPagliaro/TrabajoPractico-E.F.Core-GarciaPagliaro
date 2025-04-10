
using EF.Consola.Validators;
using EF.Data;
using EF.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace EF.Consola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //CreateDb();
            do
            {
                Console.Clear();
                Console.WriteLine("Main Menu: ");
                Console.WriteLine("1 - Developers");
                Console.WriteLine("2 - Games");
                Console.WriteLine("3 - Exit");
                Console.Write("Select an option: ");
                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        DevelopersMenu();
                        break;
                    case "2":
                        GamesMenu();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }

            } while (true);
        }

        private static void DeleteGames()
        {
            Console.Clear();
            Console.WriteLine("Deleting Games");
            Console.WriteLine("List of Games to Delete: ");
            using (var context=new GameDeveloperContext())
            {
                var games = context.Games
                    .OrderBy(g=>g.Id)
                    .Select(g => new
                {
                    g.Id,
                    g.Title,
                    g.Developer
                }).ToList();
                foreach (var gam in games)
                {
                    Console.WriteLine($"{gam.Id} - {gam.Title}");
                }
                Console.Write("Select GameID to Delete (0 to Escape): ");
                if (!int.TryParse(Console.ReadLine(), out var gameId) || gameId <= 0)
                {
                    Console.WriteLine("Invalid GameID");
                    Console.ReadLine();
                    return;
                }
                if (gameId==0)
                {
                    Console.WriteLine("Operstion cancelled by user");
                    Console.ReadLine();
                    return;
                }
                var deleteGame = context.Games.Find(gameId);
                if (deleteGame is null)
                {
                    Console.WriteLine("Game doesn't exist!!!");
                }
                else
                {
                    context.Games.Remove(deleteGame);
                    context.SaveChanges();
                    Console.WriteLine("Game successfully deleted!!");
                }
                Console.ReadLine();
                return;
            }
        }

        private static void GamesMenu()
        {
            Console.Clear();
            Console.WriteLine("GAMES: ");
            Console.WriteLine("1 - List of Games");
            Console.WriteLine("2 - Add New Game");
            Console.WriteLine("3 - Delete a Game");
            //Console.WriteLine("4 - Edit a Game");
            Console.WriteLine("4 - Back to Main Menu");
            Console.Write("Select an option: ");
            var opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    ListGames();
                    break;
                case "2":
                    AddGames();
                    break;
                case "3":
                    DeleteGames();
                    break;
                case "4":
                    
                    return;
                    
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        private static void AddGames()
        {
            Console.Clear();
            Console.WriteLine("Adding new game: ");
            Console.Write("Enter game's title: ");
            var title = Console.ReadLine();
            Console.Write("Enter game's genre: ");
            var genre = Console.ReadLine();
            Console.Write("Enter game's price: ");
            var price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter game's publish date (dd/MM/yyyy): ");
            if(!DateOnly.TryParse(Console.ReadLine(),out var publishDate))
            {
                Console.WriteLine("Invalid date");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("List of Developers to Select: ");
            using (var context = new GameDeveloperContext())
            {
                var developersList = context.Developers
                    .OrderBy(d => d.Id)
                    .ToList();
                foreach (var developer in developersList)
                {
                    Console.WriteLine($"{developer.Id} - {developer.Name}");
                }
                Console.WriteLine("Enter DeveloperID (0 New Developer): ");
                if(!int.TryParse(Console.ReadLine(), out var developerId) || developerId < 0)
                {
                    Console.WriteLine("Invalid DeveloperID");
                    Console.ReadLine();
                    return;
                }
                var selectedDeveloper = context.Developers.Find(developerId);
                if (selectedDeveloper is null)
                {
                    Console.WriteLine("Developer not found");
                    Console.ReadLine();
                    return;
                }
                var newGame = new Game
                {
                    Title = title ?? string.Empty,
                    Genre = genre,
                    Price = price,
                    PublishDate = publishDate,
                    DeveloperId = developerId
                };

                var gameValidator = new GamesValidator();
                var validationResult = gameValidator.Validate(newGame);

                if (validationResult.IsValid)
                {
                    //bool exist=context.Games.Any(g => g.Title.ToLower() == title.ToLower() &&
                    //g.DeveloperId==developerId);

                    var existingGame = context.Games.FirstOrDefault(g => g.Title.ToLower() == title.ToLower() && g.DeveloperId == developerId);

                    if (existingGame is null)
                    {
                        context.Games.Add(newGame);
                        context.SaveChanges();
                        Console.WriteLine("Game has been added!!");
                    }
                    else
                    {
                        Console.WriteLine("Game duplicated!!");
                    }

                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
                Console.ReadLine();
                return;
            }
        }

        private static void ListGames()
        {
            Console.Clear();
            Console.WriteLine("List of games");
            using (var context = new GameDeveloperContext())
            {
                var games = context.Games
                    .Include(g => g.Developer)
                    .Select(g=> new
                    {
                        g.Id,
                        g.Title,
                        g.Developer
                    })
                    .OrderBy(g => g.Title)
                    .AsNoTracking()
                    .ToList();

                foreach (var g in games)
                {
                    Console.WriteLine($"{g.Title} | Developer: {g.Developer.Name.ToUpper()}");
                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
            }

        }

        private static void DevelopersMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("DEVELOPERS: ");
                Console.WriteLine("1 - List of Developers");
                Console.WriteLine("2 - Add New Developer");
                Console.WriteLine("3 - Delete a Developer");
                Console.WriteLine("4 - Edit a Developer");
                Console.WriteLine("5 - Back to Main Menu");
                Console.Write("Select an option: ");
                var opcion = Console.ReadLine();
                switch (opcion)
                {
                    case "1":
                        ListDevelopers();
                        break;
                    case "2":
                        AddDeveloper();
                        break;
                    case "3":
                        DeleteDeveloper();
                        break;
                    case "4":
                        EditDeveloper();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            } while (true);
        }

        private static void EditDeveloper()
        {
            Console.Clear();
            Console.WriteLine("Edit Developer: ");
            using (var context = new GameDeveloperContext())
            {
                var developers = context.Developers
                    .OrderBy(d => d.Id)
                    .ToList();

                foreach (var developer in developers)
                {
                    Console.WriteLine($"{developer.Id} - {developer.Name}");
                }

                Console.Write("Enter a DeveloperID to edit: ");
                int developerId;
                if (!int.TryParse(Console.ReadLine(), out developerId) || developerId <= 0)
                {
                    Console.WriteLine("Invalid DeveloperID!!!");
                    Console.ReadLine();
                    return;
                }

                var developerInDb = context.Developers.Find(developerId);
                if (developerInDb == null)
                {
                    Console.WriteLine("The developer doesn't exist!!!");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine($"Current Developer Name: {developerInDb.Name}");
                Console.Write("Enter New Name (or ENTER to keep the same): ");
                var newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                {
                    developerInDb.Name = newName;
                }

                Console.WriteLine($"Current Foundation Date: {developerInDb.FoundationDate}");
                Console.Write("Enter New Foundation Date (dd/MM/yyyy) (or ENTER to keep the same): ");
                var newFoundationDate = Console.ReadLine();
                if (!string.IsNullOrEmpty(newFoundationDate))
                {
                    developerInDb.FoundationDate = DateOnly.ParseExact(newFoundationDate, "dd/MM/yyyy");
                }

                Console.WriteLine($"Current Country: {developerInDb.Country}");
                Console.Write("Enter New Country (or ENTER to keep the same): ");
                var newCountry = Console.ReadLine();
                if (!string.IsNullOrEmpty(newCountry))
                {
                    developerInDb.Country = newCountry;
                }

                var originalDeveloper = context.Developers
                    .AsNoTracking()
                    .FirstOrDefault(d => d.Id == developerInDb.Id);

                Console.Write($"Are you sure to edit \"{originalDeveloper!.Name}\"? (y/n): ");
                var confirm = Console.ReadLine();
                if (confirm?.ToLower() == "y")
                {
                    context.SaveChanges();
                    Console.WriteLine("Developer successfully edited!");
                }
                else
                {
                    Console.WriteLine("Operation cancelled by user.");
                }

                Console.ReadLine();
                return;
            }
        }

        private static void DeleteDeveloper()
        {
            Console.Clear();
            Console.WriteLine("Delete Developer: ");
            using (var context = new GameDeveloperContext())
            {
                var developers = context.Developers
                    .OrderBy(d => d.Id).ToList();

                foreach (var developer in developers)
                {
                    Console.WriteLine($"{developer.Id} - {developer.Name}");
                }
                Console.Write("Enter a DeveloperID to delete: ");
                int developerId;
                if(!int.TryParse(Console.ReadLine(), out developerId) || developerId <= 0) 
                {
                    Console.WriteLine("Invalid DeveloperID");
                    Console.ReadLine();
                    return;
                }

                var developerInDb = context.Developers.Find(developerId);
                if (developerInDb == null)
                {
                    Console.WriteLine("The developer doesn't exist!!!");
                    Console.ReadLine();
                    return;
                }
                var hasGames=context.Games.Any(g=>g.DeveloperId == developerInDb.Id);
                if (!hasGames)
                {
                    Console.Write($"Are you sure to delete \"{developerInDb.Name}\"? (y/n)");
                    var confirm = Console.ReadLine();
                    if (confirm?.ToLower() == "y")
                    {
                        context.Developers.Remove(developerInDb);
                        context.SaveChanges();
                        Console.WriteLine("Developer successfully removed!!");
                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled by user!!!");
                    }

                }
                else
                {
                    Console.WriteLine("The developer has games associated, cannot be deleted!!");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();
                    return;
                }
                Console.ReadLine();
                return;
            }
        }

        private static void AddDeveloper()
        {
            Console.Clear();
            Console.WriteLine("Add New Developer: ");
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("Foundation Date (dd/MM/yyyy): ");
            var foundationDate = DateOnly.ParseExact(Console.ReadLine(), "dd/MM/yyyy");
            Console.Write("Country: ");
            var country = Console.ReadLine();

            using (var context = new GameDeveloperContext())
            {
                bool exist = context.Developers.Any(d => d.Name == name);

                if (!exist)
                {
                    var developer = new Developer
                    {
                        Name = name,
                        FoundationDate = foundationDate,
                        Country = country
                    };

                    var validationContext = new ValidationContext(developer);
                    var errorMessages = new List<ValidationResult>();

                    bool isValid = Validator.TryValidateObject(developer, validationContext, errorMessages, true);

                    if (isValid)
                    {
                        context.Developers.Add(developer);
                        context.SaveChanges();
                        Console.WriteLine("Developer added!!");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                    }
                    else
                    {
                        foreach (var error in errorMessages)
                        {
                            Console.WriteLine(error.ErrorMessage);
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Developer already exists!!");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();
                    return;
                }
            }
        }

        private static void ListDevelopers()
        {
            Console.Clear();
            Console.WriteLine("List of Developers: ");
            using (var context = new GameDeveloperContext())
            {
                var developers = context.Developers
                    .OrderBy(d=>d.Name)
                    .AsNoTracking()
                    .ToList();
                foreach (var developer in developers)
                {
                    Console.WriteLine(developer);
                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
            }
        }

        private static void CreateDb()
        {
            using (var context = new GameDeveloperContext())
            {
                context.Database.EnsureCreated();
            }
            Console.WriteLine("Database created!!");
        }
    }
}
