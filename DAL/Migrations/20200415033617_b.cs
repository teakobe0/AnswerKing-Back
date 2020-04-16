using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "University",
                table: "ClassTest",
                newName: "UniversityTest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniversityTest",
                table: "ClassTest",
                newName: "University");
        }
    }
}
