
using EF.Consola.Validators;
using EF.Data;
using EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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
            using (var context = new GameDeveloperContext())
            {
                var games = context.Games
                    .OrderBy(g => g.Id)
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
                if (gameId == 0)
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
            Console.WriteLine("4 - Edit a Game");
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
                    EditGames();
                    return;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        private static void EditGames()
        {
            Console.Clear();
            Console.WriteLine("Editing Games");
            Console.WriteLine("list Of Games to Edit");
            using (var context = new GameDeveloperContext())
            {
                var games = context.Games.OrderBy(b => b.Id)
                    .Select(b => new
                    {
                        b.Id,
                        b.Title
                    }).ToList();
                foreach (var game in games)
                {
                    Console.WriteLine($"{game.Id} - {game.Title}");
                }
                Console.Write("Enter GameID to edit (0 to Escape):");
                int gameId = int.Parse(Console.ReadLine()!);
                if (gameId < 0)
                {
                    Console.WriteLine("Invalid GameID... ");
                    Console.ReadLine();
                    return;
                }
                if (gameId == 0)
                {
                    Console.WriteLine("Cancelled by user");
                    Console.ReadLine();
                    return;
                }
                var gameInDb = context.Games.Include(b => b.Developer)
                .FirstOrDefault(b => b.Id == gameId);
                if (gameInDb == null)
                {
                    Console.WriteLine("Game does not exist...");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine($"Current Game Title: {gameInDb.Title}");
                Console.Write("Enter New Title (or ENTER to Keep the same):");
                var newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle))
                {
                    gameInDb.Title = newTitle;
                }
                Console.WriteLine($"Current Game Genre: {gameInDb.Genre}");
                Console.Write("Enter New Genre (or ENTER to Keep the same):");
                var newGenre = Console.ReadLine();
                if (!string.IsNullOrEmpty(newGenre))
                {
                    gameInDb.Genre = newGenre;
                }
                Console.WriteLine($"Current Game Price: {gameInDb.Price}");
                Console.Write("Enter New Price (or ENTER to Keep the same):");
                var newPrice = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPrice))
                {
                    gameInDb.Price = decimal.Parse(newPrice);
                }
                Console.WriteLine($"Current Game Publish Date: {gameInDb.PublishDate}");
                Console.Write("Enter New Game Publish Date (or ENTER to Keep the same):");
                var newDate = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDate))
                {
                    if (!DateOnly.TryParse(newDate, out DateOnly publishDate) ||
                        publishDate > DateOnly.FromDateTime(DateTime.Today))
                    {
                        Console.WriteLine("Invalid Publish Date...");
                        Console.ReadLine();
                        return;
                    }
                    gameInDb.PublishDate = publishDate;
                }
                Console.WriteLine($"Current Game Developer:{gameInDb.Developer}");
                Console.WriteLine("Available Developers");
                var developers = context.Developers
                    .OrderBy(a => a.Id)
                    .ToList();
                foreach (var developer in developers)
                {
                    Console.WriteLine($"{developer.Id}-{developer}");
                }
                Console.Write("Enter DeveloperID (or ENTER to Keep the same or 0 New Developer):");
                var newDeveloper = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDeveloper))
                {
                    if (!int.TryParse(newDeveloper, out int developerId) || developerId < 0)
                    {
                        Console.WriteLine("You enter an invalid DeveloperID");
                        Console.ReadLine();
                        return;
                    }
                    if (developerId > 0)
                    {
                        var existDeveloper = context.Developers.Any(a => a.Id == developerId);
                        if (!existDeveloper)
                        {
                            Console.WriteLine("DeveloperID not found");
                            Console.ReadLine();
                            return;
                        }
                        gameInDb.DeveloperId = developerId;

                    }
                    else
                    {
                        //Entering new developer
                        Console.WriteLine("Adding a New Developer");
                        Console.Write("Enter Developer Team's Name:");
                        var name = Console.ReadLine();
                        var existingDeveloper = context.Developers.FirstOrDefault(
                                a => a.Name.ToLower() == name!.ToLower());
                        Console.Write("Enter Foundation Date (dd/MM/yyyy):");
                        var foundationDate = DateOnly.ParseExact(Console.ReadLine(), "dd/MM/yyyy");
                        Console.Write("Enter Country:");
                        var country = Console.ReadLine();


                        if (existingDeveloper is not null)
                        {
                            Console.WriteLine("You have entered an existing developer!!!");
                            Console.WriteLine("Assigning his DeveloperID");

                            gameInDb.DeveloperId = existingDeveloper.Id;
                        }
                        else
                        {
                            Developer Developer = new Developer
                            {
                                Name = name ?? string.Empty,
                                FoundationDate = foundationDate,
                                Country = country ?? string.Empty
                            };

                            var validationContext = new ValidationContext(Developer);
                            var errorMessages = new List<ValidationResult>();

                            bool isValid = Validator.TryValidateObject(Developer, validationContext, errorMessages, true);

                            if (isValid)
                            {
                                //gameInDb.Developer = Developer;
                                //Alternativa
                                context.Developers.Add(Developer);
                                context.SaveChanges();
                                gameInDb.DeveloperId = Developer.Id;
                            }
                            else
                            {
                                foreach (var message in errorMessages)
                                {
                                    Console.WriteLine(message);
                                }
                            }

                        }
                    }

                }

                var originalGame = context.Games
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id == gameInDb.Id);

                Console.Write($"Are you sure to edit \"{originalGame!.Title}\"? (y/n):");
                var confirm = Console.ReadLine();
                try
                {
                    if (confirm?.ToLower() == "y")
                    {
                        context.SaveChanges();
                        Console.WriteLine("Game successfully edited");
                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled by user");
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
                return;


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
            Console.Write("Enter game's price(USD): ");
            if (!decimal.TryParse(Console.ReadLine(), out var price))
            {
                Console.WriteLine("Invalid price");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter game's publish date (dd/MM/yyyy): ");
            if (!DateOnly.TryParse(Console.ReadLine(), out var publishDate))
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
                if (!int.TryParse(Console.ReadLine(), out var developerId) || developerId < 0)
                {
                    Console.WriteLine("Invalid DeveloperID");
                    Console.ReadLine();
                    return;
                }

                Developer? selectedDeveloper = null;

                if (developerId > 0)
                {
                    selectedDeveloper = context.Developers.Find(developerId);
                    if (selectedDeveloper is null)
                    {
                        Console.WriteLine("Developer not found");
                        Console.ReadLine();
                        return;
                    }
                }
                else
                {
                    // Adding a new developer
                    Console.WriteLine("Adding a New Developer");
                    Console.Write("Enter Developer Team's Name: ");
                    var name = Console.ReadLine();
                    Console.Write("Enter Foundation Date (dd/MM/yyyy): ");
                    if (!DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var foundationDate))
                    {
                        Console.WriteLine("Invalid Foundation Date");
                        Console.ReadLine();
                        return;
                    }
                    Console.Write("Enter Country: ");
                    var country = Console.ReadLine();

                    var existingDeveloper = context.Developers.FirstOrDefault(
                        d => d.Name.ToLower() == name!.ToLower());

                    if (existingDeveloper is not null)
                    {
                        Console.WriteLine("You have entered an existing developer!!!");
                        Console.WriteLine("Assigning their DeveloperID");
                        selectedDeveloper = existingDeveloper;
                    }
                    else
                    {
                        var newDeveloper = new Developer
                        {
                            Name = name ?? string.Empty,
                            FoundationDate = foundationDate,
                            Country = country ?? string.Empty
                        };

                        var validationContext = new ValidationContext(newDeveloper);
                        var errorMessages = new List<ValidationResult>();

                        bool isValid = Validator.TryValidateObject(newDeveloper, validationContext, errorMessages, true);

                        if (isValid)
                        {
                            context.Developers.Add(newDeveloper);
                            context.SaveChanges();
                            selectedDeveloper = newDeveloper;
                        }
                        else
                        {
                            foreach (var message in errorMessages)
                            {
                                Console.WriteLine(message.ErrorMessage);
                            }
                            Console.ReadLine();
                            return;
                        }
                    }
                }

                var newGame = new Game
                {
                    Title = title ?? string.Empty,
                    Genre = genre,
                    Price = price,
                    PublishDate = publishDate,
                    DeveloperId = selectedDeveloper!.Id
                };

                var gameValidator = new GamesValidator();
                var validationResult = gameValidator.Validate(newGame);

                if (validationResult.IsValid)
                {
                    var existingGame = context.Games.FirstOrDefault(g => g.Title.ToLower() == title.ToLower() && g.DeveloperId == selectedDeveloper.Id);

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
                    context.Entry(developerInDb).Collection(a => a.Games!).Load();
                    foreach (var book in developerInDb.Games!)
                    {
                        Console.WriteLine($"{book.Title}");
                    }

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
            Console.WriteLine("Enter New Developer Team's Name: ");
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
