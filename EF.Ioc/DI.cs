using EF.Data;
using EF.Data.Interfaces;
using EF.Data.Repositories;
using EF.Services.Interfaces;
using EF.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication.ExtendedProtection;

namespace EF.Ioc
{
    public static class DI
    {
        public static IServiceProvider ConfigureDI()
        {
            var services = new ServiceCollection();
            var connectionString = @"Data Source=JULIAN\\SQLEXPRESS; Initial Catalog=GamesServiceDb; Trusted_Connection=true; TrustServerCertificate=true;";

            services.AddDbContext<GameDeveloperContext>(options =>
                     options.UseSqlServer(connectionString));

            services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            services.AddScoped<IDeveloperService, DeveloperService>();

            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameService, GameService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services.BuildServiceProvider();
        }
    }
}
