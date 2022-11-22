using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnup.Migrations
{
    public partial class AddEstablishmentCodeToEstablishment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstablishmentCode",
                table: "Establishments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Establishments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstablishmentCode",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Establishments");
        }
    }
}
