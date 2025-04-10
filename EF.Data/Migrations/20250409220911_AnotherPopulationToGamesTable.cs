using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class AnotherPopulationToGamesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "DeveloperId", "Genre", "Price", "PublishDate", "Title" },
                values: new object[] { 1, 2, "Real-time strategy", 59.99m, new DateOnly(2004, 7, 1), "Warcraft III - The Frozen Throne" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
