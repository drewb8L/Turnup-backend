using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnup.Migrations
{
    public partial class AddJumotronAndChangeImgUrlToLogoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "Establishments",
                newName: "LogoUrl");

            migrationBuilder.AddColumn<string>(
                name: "JumbotronImgUrl",
                table: "Establishments",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JumbotronImgUrl",
                table: "Establishments");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Establishments",
                newName: "ImgUrl");
        }
    }
}
