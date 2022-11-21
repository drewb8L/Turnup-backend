using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnup.Migrations
{
    public partial class AddPKIdToOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderCustomerId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrderCustomerId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "OrderCustomerId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartItems");

            migrationBuilder.AddColumn<string>(
                name: "OrderCustomerId",
                table: "CartItems",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrderCustomerId",
                table: "CartItems",
                column: "OrderCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderCustomerId",
                table: "CartItems",
                column: "OrderCustomerId",
                principalTable: "Orders",
                principalColumn: "CustomerId");
        }
    }
}
