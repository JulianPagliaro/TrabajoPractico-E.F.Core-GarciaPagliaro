using EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF.Data
{
    public class GameDeveloperContext:DbContext
    {
        public GameDeveloperContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Developer> Developers { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=GamesServiceDb; Trusted_Connection=true; TrustServerCertificate=true;");
            //    .EnableSensitiveDataLogging() // permite ver valores en las consultas
            //    .LogTo(Console.WriteLine, LogLevel.Information); //muestra los sql en la consola
        }
        //FLUENT API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Game>(entity =>
            //{
            //    entity.ToTable("Games");
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Title).HasMaxLength(300)
            //    .IsRequired();
            //    entity.Property(e => e.Genre).HasMaxLength(100)
            //    .IsRequired();
            //    entity.Property(e => e.PublishDate).HasColumnType("Date")
            //    .IsRequired();
            //    entity.Property(e => e.Price).HasColumnType("Decimal(18,2)")
            //    .IsRequired();
            //    entity.HasIndex(e => new { e.Title, e.DeveloperId },"IX_Games_Title_DeveloperId").IsUnique();
            //    entity.HasOne(e => e.Developer).WithMany(e => e.Games).HasForeignKey(e => e.DeveloperId)
            //    .OnDelete(DeleteBehavior.ClientNoAction);

            //var gameList = new List<Game>
            //{
            //    new Game{
            //        Id = 1,
            //        Title = "Warcraft III - The Frozen Throne",
            //        Genre = "Real-time strategy",
            //        PublishDate = new DateOnly(2004, 7, 1),
            //        Price = 59.99M,
            //        DeveloperId = 2
            //    },
            //};
            //entity.HasData(gameList);
            //});
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameDeveloperContext).Assembly);

        }
    }
}
