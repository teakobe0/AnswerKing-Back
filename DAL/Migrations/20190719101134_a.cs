using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "University",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Order",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Notice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "ImportRecords",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Focus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Comment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Client",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "ClassWeekType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "ClassWeek",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "ClassInfo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Class",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "University");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Notice");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "ImportRecords");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Focus");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "ClassWeekType");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "ClassWeek");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "ClassInfo");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Class");
        }
    }
}
