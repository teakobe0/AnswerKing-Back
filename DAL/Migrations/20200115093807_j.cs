using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class j : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassInfoContentTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameUrl = table.Column<string>(nullable: true),
                    Contents = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ClassWeek = table.Column<int>(nullable: false),
                    ClassWeekType = table.Column<int>(nullable: false),
                    ClassTestId = table.Column<int>(nullable: false),
                    ClassInfoTestId = table.Column<int>(nullable: false),
                    UniversityTestId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassInfoContentTest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassInfoTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    ClassTestId = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassInfoTest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UniversityTestId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Professor = table.Column<string>(nullable: true),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UniversityTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityTest", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassInfoContentTest");

            migrationBuilder.DropTable(
                name: "ClassInfoTest");

            migrationBuilder.DropTable(
                name: "ClassTest");

            migrationBuilder.DropTable(
                name: "UniversityTest");
        }
    }
}
