using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddyApp.Migrations
{
    /// <inheritdoc />
    public partial class DodajGodinuISmjerStudija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GodinaStudija",
                table: "Predmet",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SmjerStudija",
                table: "Predmet",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GodinaStudija",
                table: "Predmet");

            migrationBuilder.DropColumn(
                name: "SmjerStudija",
                table: "Predmet");
        }
    }
}
