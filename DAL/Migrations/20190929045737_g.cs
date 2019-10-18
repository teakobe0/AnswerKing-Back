using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class g : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OriginClassId",
                table: "ClassInfoContent",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginUniversityId",
                table: "ClassInfoContent",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginClassId",
                table: "ClassInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginUniversityId",
                table: "Class",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginClassId",
                table: "ClassInfoContent");

            migrationBuilder.DropColumn(
                name: "OriginUniversityId",
                table: "ClassInfoContent");

            migrationBuilder.DropColumn(
                name: "OriginClassId",
                table: "ClassInfo");

            migrationBuilder.DropColumn(
                name: "OriginUniversityId",
                table: "Class");
        }
    }
}
