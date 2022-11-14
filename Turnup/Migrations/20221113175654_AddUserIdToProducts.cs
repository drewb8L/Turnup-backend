using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnup.Migrations
{
    public partial class AddUserIdToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "Products");
        }
    }
}
