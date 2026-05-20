using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddyApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImplementacijaKorisnickogModela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obavjestenje_Korisnik_KorisnikId",
                table: "Obavjestenje");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijavaNaSesiju_Korisnik_KorisnikId",
                table: "PrijavaNaSesiju");

            migrationBuilder.DropForeignKey(
                name: "FK_Prisustvo_Korisnik_KorisnikId",
                table: "Prisustvo");

            migrationBuilder.DropForeignKey(
                name: "FK_SesijaUcenja_Korisnik_KreatorId",
                table: "SesijaUcenja");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.AlterColumn<string>(
                name: "KreatorId",
                table: "SesijaUcenja",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Prisustvo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "PrijavaNaSesiju",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Obavjestenje",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusNaloga",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Uloga",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Obavjestenje_AspNetUsers_KorisnikId",
                table: "Obavjestenje",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijavaNaSesiju_AspNetUsers_KorisnikId",
                table: "PrijavaNaSesiju",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prisustvo_AspNetUsers_KorisnikId",
                table: "Prisustvo",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SesijaUcenja_AspNetUsers_KreatorId",
                table: "SesijaUcenja",
                column: "KreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obavjestenje_AspNetUsers_KorisnikId",
                table: "Obavjestenje");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijavaNaSesiju_AspNetUsers_KorisnikId",
                table: "PrijavaNaSesiju");

            migrationBuilder.DropForeignKey(
                name: "FK_Prisustvo_AspNetUsers_KorisnikId",
                table: "Prisustvo");

            migrationBuilder.DropForeignKey(
                name: "FK_SesijaUcenja_AspNetUsers_KreatorId",
                table: "SesijaUcenja");

            migrationBuilder.DropColumn(
                name: "Ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prezime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusNaloga",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Uloga",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "KreatorId",
                table: "SesijaUcenja",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikId",
                table: "Prisustvo",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikId",
                table: "PrijavaNaSesiju",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikId",
                table: "Obavjestenje",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    IdKorisnika = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusNaloga = table.Column<int>(type: "int", nullable: false),
                    UlogaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.IdKorisnika);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Obavjestenje_Korisnik_KorisnikId",
                table: "Obavjestenje",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnika",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijavaNaSesiju_Korisnik_KorisnikId",
                table: "PrijavaNaSesiju",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnika",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prisustvo_Korisnik_KorisnikId",
                table: "Prisustvo",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnika",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SesijaUcenja_Korisnik_KreatorId",
                table: "SesijaUcenja",
                column: "KreatorId",
                principalTable: "Korisnik",
                principalColumn: "IdKorisnika",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
