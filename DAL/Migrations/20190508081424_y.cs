using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class y : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsHide",
                table: "University",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHide",
                table: "University");
        }
    }
}
