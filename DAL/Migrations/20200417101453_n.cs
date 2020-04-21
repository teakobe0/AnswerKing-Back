using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class n : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "ClassInfoTest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "ClassInfoContentTest",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "ClassInfoTest");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "ClassInfoContentTest");
        }
    }
}
