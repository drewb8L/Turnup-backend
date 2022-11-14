using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnup.Migrations
{
    public partial class RemoveMenuFromEstablishmentAddProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishments_Menu_MenuId",
                table: "Establishments");

            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropIndex(
                name: "IX_Establishments_MenuId",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Establishments");

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_EstablishmentId",
                table: "Products",
                column: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Establishments_EstablishmentId",
                table: "Products",
                column: "EstablishmentId",
                principalTable: "Establishments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Establishments_EstablishmentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_EstablishmentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EstablishmentId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Establishments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablishmentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItem_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_MenuId",
                table: "Establishments",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_MenuId",
                table: "MenuItem",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_ProductId",
                table: "MenuItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishments_Menu_MenuId",
                table: "Establishments",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
