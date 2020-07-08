using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class qa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Answer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Answer");
        }
    }
}
