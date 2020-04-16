using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class v : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UniversityTest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UniversityTest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "UniversityTest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Intro",
                table: "UniversityTest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "UniversityTest",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Integral",
                table: "Client",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Memo",
                table: "ClassTest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "ClassTest",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoUse",
                table: "ClassInfoTest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Use",
                table: "ClassInfoTest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IntegralRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    Integral = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegralRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegralRecords");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UniversityTest");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UniversityTest");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "UniversityTest");

            migrationBuilder.DropColumn(
                name: "Intro",
                table: "UniversityTest");

            migrationBuilder.DropColumn(
                name: "State",
                table: "UniversityTest");

            migrationBuilder.DropColumn(
                name: "Integral",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Memo",
                table: "ClassTest");

            migrationBuilder.DropColumn(
                name: "University",
                table: "ClassTest");

            migrationBuilder.DropColumn(
                name: "NoUse",
                table: "ClassInfoTest");

            migrationBuilder.DropColumn(
                name: "Use",
                table: "ClassInfoTest");
        }
    }
}
