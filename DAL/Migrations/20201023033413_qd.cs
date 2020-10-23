using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class qd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgContent",
                table: "Question",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgContent",
                table: "Question");
        }
    }
}
