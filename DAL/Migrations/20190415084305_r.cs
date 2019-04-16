using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class r : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "ClassInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ClassInfo");
        }
    }
}
