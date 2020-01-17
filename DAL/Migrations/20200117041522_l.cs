using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class l : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClassWeekType",
                table: "ClassInfoContentTest",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClassWeekType",
                table: "ClassInfoContentTest",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
