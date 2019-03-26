using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class jr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsAudit",
                table: "ClassInfoContent",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAudit",
                table: "ClassInfoContent");
        }
    }
}
