using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class PopulateGamesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('Warcraft III - The Frozen Throne', 'Real-time strategy', '2004-07-01', 59.99, 2)");
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('The Witcher 3: Wild Hunt', 'Action role-playing', '2015-05-19', 39.99, 6)");
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('Dark Souls III', 'Action role-playing', '2016-03-24', 49.99, 1)");
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('Metal Gear Solid', 'Action-adventure', '1998-09-03', 49.99, 4)");
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('Half-Life 2', 'First-person shooter', '2004-11-16', 59.99, 5)");
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('Cyberpunk 2077', 'Action role-playing', '2020-12-10', 59.99, 6)");
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('Elden Ring', 'Action role-playing', '2022-02-25', 59.99, 1)");
            migrationBuilder.Sql("INSERT INTO Games (Title, Genre, PublishDate, Price, DeveloperId) VALUES ('Monster Hunter Wilds', 'Action role-playing', '2025-02-28', 59.99, 3)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Games WHERE Id in (1,2,3,4,5,6,7,8)");
        }
    }
}
