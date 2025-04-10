using Microsoft.EntityFrameworkCore.Migrations;
using System.Globalization;

#nullable disable

namespace EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class PopulateDevelopersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Developers",
                columns: new[] {"Id","Name", "FoundationDate", "Country",},
                values: new object[,]
                {
                    {1, "FromSoftware",DateOnly.ParseExact("01/01/1986", "dd/MM/yyyy", CultureInfo.InvariantCulture), "Japan" },
                    { 2, "Blizzard",DateOnly.ParseExact("08/02/1991", "dd/MM/yyyy", CultureInfo.InvariantCulture), "USA" },
                    { 3, "Capcom",DateOnly.ParseExact("30/05/1979", "dd/MM/yyyy", CultureInfo.InvariantCulture), "Japan" },
                    { 4, "Konami",DateOnly.ParseExact("21/03/1969", "dd/MM/yyyy", CultureInfo.InvariantCulture), "Japan" },
                    { 5, "Valve Corporation",DateOnly.ParseExact("24/08/1996", "dd/MM/yyyy", CultureInfo.InvariantCulture), "USA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Developers",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5 });
        }
    }
}
