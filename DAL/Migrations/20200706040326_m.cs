using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class m : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sign",
                table: "Answer");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Question",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sign",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Notice",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Sign",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notice");

            migrationBuilder.AddColumn<int>(
                name: "Sign",
                table: "Answer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
