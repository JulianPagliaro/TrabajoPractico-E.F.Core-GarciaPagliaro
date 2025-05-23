using EF.Services.DTOs.Developer;
using EF.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Consola
{
    public class DeveloperMenu
    {
        private readonly IServiceProvider _serviceProvider = null!;

        public DeveloperMenu(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("DEVELOPERS: ");
            Console.WriteLine("1 - List of Developers");
            Console.WriteLine("2 - Add New Developer");
            Console.WriteLine("3 - Edit Developer");
            Console.WriteLine("4 - Delete Developer");
            Console.WriteLine("5 - List of Developers With Games");
            Console.WriteLine("6 - Developers With Games(Summary or Details");
            Console.WriteLine("7 - Return");
            Console.WriteLine("Select an option: ");
            var option = Console.ReadLine();
            using (var scope = _serviceProvider.CreateScope())
            {
                var developerService = scope.ServiceProvider
                    .GetRequiredService<IDeveloperService>();
                switch (option)
                {
                    case "1":
                        DeveloperList(developerService);
                        break;
                    case "2":
                        AddDeveloper(developerService);
                        break;
                    case "3":
                        EditDeveloper(developerService);
                        break;
                    case "4":
                        DeleteDeveloper(developerService);
                        break;
                    case "5":
                        ListOfDDevelopersWithGames(developerService);
                        break;
                    case "6":
                        ListOfDevelopersWithGamesSummary(developerService);
                        break;
                }
            }
        }

        private void EditDeveloper(IDeveloperService developerService)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Edit A Developer");
                var developers = developerService.GetAll("Id");
                foreach (var developer in developers)
                {
                    Console.WriteLine($"{developer.Id} - {developer.Name} - {developer.Country}");
                }

                Console.Write("Enter a DeveloperID to edit (0 to Escape):");
                int developerId;
                if (!int.TryParse(Console.ReadLine(), out developerId) || developerId < 0)
                {
                    Console.WriteLine("Invalid DeveloperID!!!");
                    Console.ReadLine();
                    return;
                }
                if (developerId == 0) return;

                var developerInDb = developerService.GetById(developerId);
                if (developerInDb != null)
                {
                    Console.WriteLine($"Current Developer Name: {developerInDb.Name}");
                    Console.Write("Enter New First Name (or ENTER to Keep the same)");
                    var newName = Console.ReadLine();
                    if (string.IsNullOrEmpty(newName))
                    {
                        newName = developerInDb.Name;
                    }
                    Console.WriteLine($"Current Author Country: {developerInDb.Country}");
                    Console.Write("Enter New Country (or ENTER to Keep the same)");
                    var newCountry = Console.ReadLine();
                    if (string.IsNullOrEmpty(newCountry))
                    {
                        newCountry = developerInDb.Country;
                    }
                    Console.WriteLine($"Current Author Foundation Date: {developerInDb.FoundationDate}");
                    Console.Write("Enter New Foundation Date (dd/MM/yyyy) (or ENTER to Keep the same)");
                    var newFoundationDate = Console.ReadLine();
                    if (string.IsNullOrEmpty(newFoundationDate))
                    {
                        newFoundationDate = developerInDb.FoundationDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        DateOnly.TryParseExact(newFoundationDate, "dd/MM/yyyy", out var foundationDate);
                        newFoundationDate = foundationDate.ToString("dd/MM/yyyy");
                    }

                    var originalDeveloper = developerService.GetById(developerId);

                    Console.Write($"Are you sure to edit \"{originalDeveloper!.Name} {originalDeveloper.Country} {originalDeveloper.FoundationDate}\"? (y/n):");
                    var confirm = Console.ReadLine();
                    if (confirm?.ToLower() == "y")
                    {
                        DeveloperUpdateDto developerUpdateDto = new DeveloperUpdateDto()
                        {
                            Id = developerInDb.Id,
                            Name = newName ?? string.Empty,
                            Country = newCountry ?? string.Empty,
                            FoundationDate = DateOnly.ParseExact(newFoundationDate, "dd/MM/yyyy")
                        };
                        if (developerService.Update(developerUpdateDto, out var errors))
                        {
                            Console.WriteLine("Author successfully updated");
                        }
                        else
                        {
                            Console.WriteLine("Errors while trying to update an author!!");
                            foreach (var message in errors)
                            {
                                Console.WriteLine(message);
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled by user");
                    }
                }
                else
                {
                    Console.WriteLine("Author does not exist");
                }
                Console.ReadLine();

            } while (true);
        }


        private void DeleteDeveloper(IDeveloperService developerService)

        {
            Console.Clear();
            Console.WriteLine("Delete A Developer");
            var developers = developerService.GetAll("Id");
            foreach (var developer in developers)
            {
                Console.WriteLine($"{developer.Id} - {developer.Name}");
            }

            Console.Write("Enter an DeveloperID to delete (0 to Escape):");
            int developerId;
            if (!int.TryParse(Console.ReadLine(), out developerId) || developerId < 0)
            {
                Console.WriteLine("Invalid DeveloperID!!!");
                Console.ReadLine();
                return;
            }
            if (developerId == 0) return;
            var developerInDb = developerService.GetById(developerId);
            if (developerInDb is null)
            {
                Console.WriteLine("ID no found!!!");
                Console.ReadLine();
                return;
            }
            Console.Write($"Are you sure to delete \"{developerInDb.Name}\"? (y/n):");
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() == "y")
            {
                if (developerService.Delete(developerId, out var errors))
                {
                    Console.WriteLine("Developer Successfully Removed");
                }
                else
                {
                    Console.WriteLine("Error while trying to delete a Developer");
                    foreach (var message in errors)
                    {
                        Console.WriteLine(message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled by user");
            }

            Console.ReadLine();

        }



        private void AddDeveloper(IDeveloperService developerService)
        {
            Console.Clear();
            Console.WriteLine("Adding a New Developer: ");
            Console.Write("Enter Name:");
            var name = Console.ReadLine();
            Console.Write("Foundation Date (dd/MM/yyyy): ");
            var foundationDate = DateOnly.ParseExact(Console.ReadLine(), "dd/MM/yyyy");
            Console.Write("Country: ");
            var country = Console.ReadLine();

            var developerDto = new DeveloperCreateDto
            {
                Name = name ?? string.Empty,
                FoundationDate = foundationDate,
                Country = country ?? string.Empty

            };


            if (developerService.Create(developerDto, out var errors))
            {
                Console.WriteLine("Developer Succesfully Added");

            }
            else
            {
                foreach (var message in errors)
                {
                    Console.WriteLine(message.ToString());
                }
            }
            Console.ReadLine();
        }

        private void ListOfDevelopersWithGamesSummary(IDeveloperService developerService)
        {
            Console.Clear();
            Console.WriteLine("List of Developers");
            Console.Write("Show (1) Summary or (2) Details? ");
            var option = Console.ReadLine();

            var developersWithGames = developerService.DevelopersGroupIdGames();
            foreach (var developer in developersWithGames)
            {
                Console.WriteLine($"{developer.Id} - {developer.Name} (Games: {developer.GamesCount})");

                if (option == "2") // Opción de detalle
                {
                    if (developer.Games!.Any())
                    {
                        Console.WriteLine("   Games:");
                        foreach (var game in developer.Games!)
                        {
                            Console.WriteLine($"     - {game.Title}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("   🚫 No Games available.");
                    }
                }
            }
            Console.ReadLine();

        }

        private void ListOfDDevelopersWithGames(IDeveloperService developerService)
        {
            Console.Clear();
            Console.WriteLine("List of Games: ");
            var developersWithGames = developerService.GetAllWithGames();
            foreach (var item in developersWithGames)
            {
                Console.WriteLine($"DeveloperId:{item.Id} - Name: {item.Name}");
                Console.WriteLine("    Games");
                if (item.Games.Count > 0)
                {
                    foreach (var game in item.Games)
                    {
                        Console.WriteLine($"        {game.Title}");
                    }

                }
                else
                {
                    Console.WriteLine("         No Games available yet");
                }
            }
            Console.WriteLine("ENTER to continue");
            Console.ReadLine();
        }

        private void DeveloperList(IDeveloperService developerService)
        {
            Console.Clear();
            Console.WriteLine("List of Developers");
            var developers = developerService.GetAll();
            foreach (var developer in developers)
            {
                Console.WriteLine($"Id: {developer.Id} - Name: {developer.Name}");
            }
            Console.WriteLine("ENTER to continue");
            Console.ReadLine();
        }
    }
}
