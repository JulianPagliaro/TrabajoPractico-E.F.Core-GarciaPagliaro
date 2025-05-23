using EF.Services.DTOs.Developer;
using EF.Services.DTOs.Game;
using EF.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.RegularExpressions;

namespace EF.Consola
{
    public class GameMenu
    {
        private readonly IServiceProvider _serviceProvider = null!;
        public GameMenu(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void MostrarMenu()
        {

            do
            {
                Console.Clear();
                Console.WriteLine("🎮 GAMES");
                Console.WriteLine("1 - List of Games");
                Console.WriteLine("2 - Add New Game");
                Console.WriteLine("3 - Delete a Game");
                Console.WriteLine("4 - Edit a Game");
                Console.WriteLine("5 - Games Group By Developer");
                Console.WriteLine("r - Return");
                Console.Write("Enter an option:");

                var option = Console.ReadLine();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var gameService = scope.ServiceProvider
                        .GetRequiredService<IGameService>();
                    var developerService = scope.ServiceProvider
                        .GetRequiredService<IDeveloperService>();
                    switch (option)
                    {
                        case "1":
                            GamesList(gameService);
                            break;
                        case "2":
                            AddGame(gameService, developerService);
                            break;
                        case "3":
                            DeleteGames(gameService);
                            break;
                        case "4":
                            EditGames(gameService, developerService);
                            break;
                        case "5":
                            GamesGroupByDeveloper(gameService);
                            break;
                        case "r":
                            return;
                        default:
                            break;
                    }

                }
            } while (true);
        }

        private void GamesGroupByDeveloper(IGameService gameService)
        {
            Console.Clear();
            Console.WriteLine("List of Games");
            var groups = gameService.GamesGroupByDeveloper();
            foreach (var group in groups)
            {
                Console.WriteLine($"DeveloperID:{group.Developer.Id} - Developer: {group.Developer.Name} - {group.Developer.Country}");
                Console.WriteLine("    Games:");
                if (group.Games is not null)
                {
                    foreach (var game in group.Games)
                    {
                        Console.WriteLine($"        {game.Title} - {game.PublishDate}");

                    }

                }
                else
                {
                    Console.WriteLine("         No fucking games!!!");
                }
            }
            Console.ReadLine();
        }

        private void EditGames(IGameService gameService, IDeveloperService developerService)
        {
            Console.Clear();
            Console.WriteLine("Editing Game");
            Console.WriteLine("List of Games to Edit");
            var games = gameService.GetAll("Id");
            foreach (var game in games)
            {
                Console.WriteLine($"{game.Id.ToString().PadLeft(4, ' ')} - {game.Title}");
            }
            Console.Write("Select GameId to Edit (0 to Escape):");
            int gameId = int.Parse(Console.ReadLine()!);
            if (gameId < 0)
            {
                Console.WriteLine("Invalid GameID... ");
                Console.ReadLine();
                return;
            }
            if (gameId == 0) return;

            var gameInDb = gameService.GetById(gameId);
            if (gameInDb == null)
            {
                Console.WriteLine("Game not found!!!");
                Console.ReadLine();
                return;
            }

            GameUpdateDto gameUpdateDto = new GameUpdateDto()
            {
                Id = gameInDb.Id,
                Title = gameInDb.Title,
                Genre = gameInDb.Genre,
                Price = gameInDb.Price,
                PublishDate = gameInDb.PublishDate,
                DeveloperId = gameInDb.DeveloperId
            };

            Console.WriteLine($"Current Game Title: {gameInDb.Title}");
            Console.Write("Enter New Title (or ENTER to Keep the same):");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrEmpty(newTitle))
            {
                gameUpdateDto.Title = newTitle;
            }

            Console.WriteLine($"Current Game Genre: {gameInDb.Genre}");
            Console.Write("Enter Game Genre (or ENTER to Keep the same):");
            var newGenre = Console.ReadLine();
            if (!string.IsNullOrEmpty(newGenre))
            {
                gameUpdateDto.Genre = newGenre;
            }

            Console.WriteLine($"Current Game Price: {gameInDb.Price}");
            Console.Write("Enter Game Price (or ENTER to Keep the same):");
            var newPrice = Console.ReadLine();
            if (!string.IsNullOrEmpty(newPrice) && decimal.TryParse(newPrice, out var price))
            {
                gameUpdateDto.Price = price;
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
                gameUpdateDto.PublishDate = publishDate;
            }

            DeveloperDto? developerDto = developerService.GetById(gameInDb.DeveloperId);
            Console.WriteLine($"Current Game Developer:{developerDto!.Name} - {developerDto.Country}");
            Console.WriteLine("Available Developers");
            var developers = developerService.GetAll("Id");
            foreach (var developer in developers)
            {
                Console.WriteLine($"{developer.Id.ToString().PadLeft(4, ' ')}-{developer.Name} - {developer.Country}");
            }

            Console.Write("Enter DeveloperID (or ENTER to Keep the same or 0 New Developer):");
            var newDeveloperId = Console.ReadLine();
            if (!string.IsNullOrEmpty(newDeveloperId))
            {
                if (!int.TryParse(newDeveloperId, out int developerId) || developerId < 0)
                {
                    Console.WriteLine("You enter an invalid DeveloperId");
                    Console.ReadLine();
                    return;
                }
                if (developerId > 0)
                {
                    var existingDeveloper = developerService.GetById(developerId);
                    if (existingDeveloper is null)
                    {
                        Console.WriteLine("DeveloperID not found");
                        Console.ReadLine();
                        return;
                    }
                    gameUpdateDto.DeveloperId = developerId;

                }
                else
                {
                    //Entering new developer
                    Console.WriteLine("Adding a New Developer: ");
                    Console.Write("Enter Name:");
                    var name = Console.ReadLine();

                    Console.Write("Foundation Date (dd/MM/yyyy): ");
                    var foundationDate = DateOnly.ParseExact(Console.ReadLine(), "dd/MM/yyyy");

                    Console.Write("Country: ");
                    var country = Console.ReadLine();

                    var existingDeveloper = developerService.GetByName(name ?? string.Empty,
                        country ?? string.Empty, foundationDate);

                    if (existingDeveloper is not null)
                    {
                        Console.WriteLine("You have entered an existing author!!!");
                        Console.WriteLine("Assigning his AuthorID");

                        gameUpdateDto.DeveloperId = existingDeveloper.Id;
                    }
                    else
                    {
                        DeveloperCreateDto newDeveloper = new DeveloperCreateDto
                        {
                            Name = name ?? string.Empty,
                            Country = country ?? string.Empty,
                            FoundationDate = foundationDate
                        };
                        DeveloperDto? developerCreated = null;
                        if (developerService.Create(newDeveloper, out developerCreated, out var developerErrors))
                        {
                            Console.WriteLine("New Developer Added Successfully");
                            gameUpdateDto.DeveloperId = developerCreated!.Id;
                        }
                        else
                        {
                            Console.WriteLine("Errors while trying to add a new developer");
                            foreach (var item in developerErrors)
                            {
                                Console.WriteLine(item);
                            }
                        }
                    }
                }
                var originalGame = gameService.GetById(gameId);

                Console.Write($"Are you sure to edit \"{originalGame!.Title}\"? (y/n):");
                var confirm = Console.ReadKey().KeyChar;
                try
                {
                    if (confirm.ToString().ToLower() == "y")
                    {
                        if (gameService.Update(gameUpdateDto, out var errors))
                        {
                            Console.WriteLine("game successfully edited");

                        }
                        else
                        {
                            Console.WriteLine("Errors while trying to update a game");
                            errors.ForEach(error => Console.WriteLine(error));
                        }
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


            }
        }



        private void DeleteGames(IGameService gameService)
        {
            Console.Clear();
            Console.WriteLine("Deleting Games");
            Console.WriteLine("List of Games to Delete");
            var games = gameService.GetAll("Id");
            foreach (var game in games)
            {
                Console.WriteLine($"{game.Id.ToString().PadLeft(4, ' ')} - {game.Title}");
            }

            Console.Write("Select GameId to Delete (0 to Escape):");
            if (!int.TryParse(Console.ReadLine(), out int gameId) || gameId < 0)
            {
                Console.WriteLine("Invalid GameID...");
                Console.ReadLine();
                return;
            }
            if (gameId == 0) return;
            Console.WriteLine($"Are you sure to delete this game? (y/n)");
            var response = Console.ReadKey().KeyChar;
            if (response.ToString().ToUpper() == "Y")
            {
                if (gameService.Delete(gameId, out var errors))
                {
                    Console.WriteLine("Game Successfully Deleted");

                }
                else
                {
                    Console.WriteLine("Errors while trying to delete a game");
                    errors.ForEach(x => Console.WriteLine(x));
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled by user");
            }
            Console.ReadLine();
        }

        private void AddGame(IGameService gameService, IDeveloperService developerService)
        {
            Console.Clear();
            Console.WriteLine("Adding New Game");
            Console.Write("Enter game's title:");
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

            Console.WriteLine("List of Developer to Select");

            var developerList = developerService!.GetAll("Id");
            foreach (var developer in developerList)
            {
                Console.WriteLine($"{developer.Id} - {developer.Name} - {developer.Country}");
            }

            Console.Write("Enter DeveloperID (0 New Developer):");
            if (!int.TryParse(Console.ReadLine(), out var developerId) || developerId < 0)
            {
                Console.WriteLine("Invalid DeveloperId....");
                Console.ReadLine();
                return;
            }
            if (developerId > 0)
            {
                var selectedDeveloper = developerService.GetById(developerId);
                if (selectedDeveloper is null)
                {
                    Console.WriteLine("Developer not found!!!");
                    Console.ReadLine();
                    return;
                }

            }
            else
            {
                //Entering new author
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

                if (developerService.Exist(name!, country!, foundationDate, null) is false)
                {
                    var existingDeveloper = developerService.GetByName(name ?? string.Empty,
                        country ?? string.Empty, foundationDate);
                    Console.WriteLine("You have entered an existing developer!!!");
                    Console.WriteLine("Assigning DeveloperID");
                    developerId = existingDeveloper!.Id;
                }
                else
                {
                    DeveloperCreateDto newDeveloper = new DeveloperCreateDto
                    {
                        Name = name ?? string.Empty,
                        Country = country ?? string.Empty,
                        FoundationDate = foundationDate
                    };
                    DeveloperDto? developerCreated = null;
                    if (developerService.Create(newDeveloper, out developerCreated, out var developerErrors))
                    {
                        Console.WriteLine("New Developer Added Successfully");
                        developerId = developerCreated!.Id;
                    }
                    else
                    {
                        Console.WriteLine("Errors while trying to add a new developer");
                        foreach (var item in developerErrors)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
            }
            GameCreateDto gameDto = new GameCreateDto()
            {
                Title = title ?? string.Empty,
                Genre = genre ?? string.Empty,
                PublishDate = publishDate,
                Price = price,
                DeveloperId = developerId

            };
            if (gameService.Create(gameDto, out var errors))
            {
                Console.WriteLine("Game Successfully Added");
            }
            else
            {
                Console.WriteLine("Errors while trying to add a new book");
                errors.ForEach(error => Console.WriteLine(error));
            }

            Console.ReadLine();
        }

        private void GamesList(IGameService gameService)
        {
            Console.Clear();
            Console.WriteLine("List of Available Books");
            var games = gameService.GetAll();
            foreach (var game in games)
            {
                Console.WriteLine($"{game.Title}");
            }
            Console.ReadLine();
        }

    }
}
