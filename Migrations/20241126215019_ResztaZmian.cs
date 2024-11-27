using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sklep.Migrations
{
    /// <inheritdoc />
    public partial class ResztaZmian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produkty_Zamowienia_ZamowienieId",
                table: "Produkty");

            migrationBuilder.DropIndex(
                name: "IX_Produkty_ZamowienieId",
                table: "Produkty");

            migrationBuilder.DropColumn(
                name: "ZamowienieId",
                table: "Produkty");

            migrationBuilder.CreateTable(
                name: "ZamowienieProdukty",
                columns: table => new
                {
                    ZamowienieId = table.Column<int>(type: "int", nullable: false),
                    ProduktId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZamowienieProdukty", x => new { x.ZamowienieId, x.ProduktId });
                    table.ForeignKey(
                        name: "FK_ZamowienieProdukty_Produkty_ProduktId",
                        column: x => x.ProduktId,
                        principalTable: "Produkty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZamowienieProdukty_Zamowienia_ZamowienieId",
                        column: x => x.ZamowienieId,
                        principalTable: "Zamowienia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZamowienieProdukty_ProduktId",
                table: "ZamowienieProdukty",
                column: "ProduktId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZamowienieProdukty");

            migrationBuilder.AddColumn<int>(
                name: "ZamowienieId",
                table: "Produkty",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produkty_ZamowienieId",
                table: "Produkty",
                column: "ZamowienieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produkty_Zamowienia_ZamowienieId",
                table: "Produkty",
                column: "ZamowienieId",
                principalTable: "Zamowienia",
                principalColumn: "Id");
        }
    }
}
