using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class qs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassWeek");

            migrationBuilder.DropTable(
                name: "ClassWeekType");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Question",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Question");

            migrationBuilder.CreateTable(
                name: "ClassWeek",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassInfoId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    RefId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassWeek", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassWeekType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassWeekId = table.Column<int>(nullable: false),
                    ClassWeekTypeId = table.Column<int>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    RefId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassWeekType", x => x.Id);
                });
        }
    }
}
