using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetAuthorsTableIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Developers_Name",
                table: "Developers",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Developers_Name",
                table: "Developers");
        }
    }
}
