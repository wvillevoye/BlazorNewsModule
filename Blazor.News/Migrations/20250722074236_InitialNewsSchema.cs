using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.News.Migrations
{
    /// <inheritdoc />
    public partial class InitialNewsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NieuwsCategorieen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NaamNl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NaamEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NieuwsCategorieen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NieuwsArtikelen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitelNl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TitelEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OndertitelNl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OndertitelEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InhoudNl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InhoudEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Auteur = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublicatieDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoofdafbeeldingUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CategorieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NieuwsArtikelen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NieuwsArtikelen_NieuwsCategorieen_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "NieuwsCategorieen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NieuwsInternalLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TekstNl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TekstEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NieuwsArtikelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NieuwsInternalLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NieuwsInternalLinks_NieuwsArtikelen_NieuwsArtikelId",
                        column: x => x.NieuwsArtikelId,
                        principalTable: "NieuwsArtikelen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NieuwsArtikelen_CategorieId",
                table: "NieuwsArtikelen",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_NieuwsInternalLinks_NieuwsArtikelId",
                table: "NieuwsInternalLinks",
                column: "NieuwsArtikelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NieuwsInternalLinks");

            migrationBuilder.DropTable(
                name: "NieuwsArtikelen");

            migrationBuilder.DropTable(
                name: "NieuwsCategorieen");
        }
    }
}
