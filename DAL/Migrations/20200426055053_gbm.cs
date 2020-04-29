using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class gbm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassInfoContentTest");

            migrationBuilder.DropTable(
                name: "ClassInfoTest");

            migrationBuilder.DropTable(
                name: "ClassTest");

            migrationBuilder.DropTable(
                name: "UniversityTest");

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UniversityId = table.Column<int>(nullable: false),
                    University = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    Professor = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    ClassId = table.Column<int>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    Use = table.Column<int>(nullable: false),
                    NoUse = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassInfoContent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameUrl = table.Column<string>(nullable: true),
                    Contents = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ClassWeek = table.Column<int>(nullable: false),
                    ClassWeekType = table.Column<string>(nullable: true),
                    ClassId = table.Column<int>(nullable: false),
                    ClassInfoId = table.Column<int>(nullable: false),
                    UniversityId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassInfoContent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "University",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Intro = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_University", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "ClassInfo");

            migrationBuilder.DropTable(
                name: "ClassInfoContent");

            migrationBuilder.DropTable(
                name: "University");

            migrationBuilder.CreateTable(
                name: "ClassInfoContentTest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassInfoTestId = table.Column<int>(nullable: false),
                    ClassTestId = table.Column<int>(nullable: false),
                    ClassWeek = table.Column<int>(nullable: false),
                    ClassWeekType = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    Contents = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameUrl = table.Column<string>(nullable: true),
                    RefId = table.Column<int>(nullable: false),
                    UniversityTestId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
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
                    ClassTestId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NoUse = table.Column<int>(nullable: false),
                    RefId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Use = table.Column<int>(nullable: false)
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
                    ClientId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Professor = table.Column<string>(nullable: true),
                    RefId = table.Column<int>(nullable: false),
                    UniversityTest = table.Column<string>(nullable: true),
                    UniversityTestId = table.Column<int>(nullable: false)
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
                    City = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Intro = table.Column<string>(nullable: true),
                    IsAudit = table.Column<bool>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RefId = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversityTest", x => x.Id);
                });
        }
    }
}
