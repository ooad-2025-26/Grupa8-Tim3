using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddyApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    IdKorisnika = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UlogaId = table.Column<int>(type: "int", nullable: false),
                    StatusNaloga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.IdKorisnika);
                });

            migrationBuilder.CreateTable(
                name: "Lokacija",
                columns: table => new
                {
                    IdLokacije = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipLokacije = table.Column<int>(type: "int", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokacija", x => x.IdLokacije);
                });

            migrationBuilder.CreateTable(
                name: "Predmet",
                columns: table => new
                {
                    IdPredmeta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Oznaka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusPredmeta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmet", x => x.IdPredmeta);
                });

            migrationBuilder.CreateTable(
                name: "Uloga",
                columns: table => new
                {
                    IdUloge = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<int>(type: "int", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uloga", x => x.IdUloge);
                });

            migrationBuilder.CreateTable(
                name: "Obavjestenje",
                columns: table => new
                {
                    IdObavjestenja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumSlanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    TipObavjestenja = table.Column<int>(type: "int", nullable: false),
                    Procitano = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obavjestenje", x => x.IdObavjestenja);
                    table.ForeignKey(
                        name: "FK_Obavjestenje_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnika",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SesijaUcenja",
                columns: table => new
                {
                    IdSesije = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Trajanje = table.Column<int>(type: "int", nullable: false),
                    LokacijaId = table.Column<int>(type: "int", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    KreatorId = table.Column<int>(type: "int", nullable: false),
                    MaksimalanBrojUcesnika = table.Column<int>(type: "int", nullable: false),
                    BrojSlobodnihMjesta = table.Column<int>(type: "int", nullable: false),
                    StatusSesije = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesijaUcenja", x => x.IdSesije);
                    table.ForeignKey(
                        name: "FK_SesijaUcenja_Korisnik_KreatorId",
                        column: x => x.KreatorId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnika",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SesijaUcenja_Lokacija_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacija",
                        principalColumn: "IdLokacije",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SesijaUcenja_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "IdPredmeta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijavaNaSesiju",
                columns: table => new
                {
                    IdPrijave = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumPrijave = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    SesijaId = table.Column<int>(type: "int", nullable: false),
                    StatusPrijave = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijavaNaSesiju", x => x.IdPrijave);
                    table.ForeignKey(
                        name: "FK_PrijavaNaSesiju_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnika",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrijavaNaSesiju_SesijaUcenja_SesijaId",
                        column: x => x.SesijaId,
                        principalTable: "SesijaUcenja",
                        principalColumn: "IdSesije",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prisustvo",
                columns: table => new
                {
                    IdPrisustva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    SesijaId = table.Column<int>(type: "int", nullable: false),
                    TrajanjeUcenja = table.Column<int>(type: "int", nullable: false),
                    StatusPrisustva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prisustvo", x => x.IdPrisustva);
                    table.ForeignKey(
                        name: "FK_Prisustvo_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "IdKorisnika",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prisustvo_SesijaUcenja_SesijaId",
                        column: x => x.SesijaId,
                        principalTable: "SesijaUcenja",
                        principalColumn: "IdSesije",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Obavjestenje_KorisnikId",
                table: "Obavjestenje",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavaNaSesiju_KorisnikId",
                table: "PrijavaNaSesiju",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavaNaSesiju_SesijaId",
                table: "PrijavaNaSesiju",
                column: "SesijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prisustvo_KorisnikId",
                table: "Prisustvo",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Prisustvo_SesijaId",
                table: "Prisustvo",
                column: "SesijaId");

            migrationBuilder.CreateIndex(
                name: "IX_SesijaUcenja_KreatorId",
                table: "SesijaUcenja",
                column: "KreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SesijaUcenja_LokacijaId",
                table: "SesijaUcenja",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_SesijaUcenja_PredmetId",
                table: "SesijaUcenja",
                column: "PredmetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obavjestenje");

            migrationBuilder.DropTable(
                name: "PrijavaNaSesiju");

            migrationBuilder.DropTable(
                name: "Prisustvo");

            migrationBuilder.DropTable(
                name: "Uloga");

            migrationBuilder.DropTable(
                name: "SesijaUcenja");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Lokacija");

            migrationBuilder.DropTable(
                name: "Predmet");
        }
    }
}
