using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class sb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "ClassInfo");

            migrationBuilder.DropTable(
                name: "ClassInfoContent");

            migrationBuilder.DropTable(
                name: "University");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Difficulty = table.Column<string>(nullable: true),
                    IsDel = table.Column<bool>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OriginUniversityId = table.Column<int>(nullable: false),
                    Professor = table.Column<string>(nullable: true),
                    RefId = table.Column<int>(nullable: false),
                    University = table.Column<string>(nullable: true),
                    UniversityId = table.Column<int>(nullable: false)
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
                    ClassId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Evaluate = table.Column<string>(nullable: true),
                    IsDel = table.Column<bool>(nullable: false),
                    NoUse = table.Column<int>(nullable: false),
                    OriginClassId = table.Column<int>(nullable: false),
                    PageViews = table.Column<int>(nullable: false),
                    RefId = table.Column<int>(nullable: false),
                    TotalGrade = table.Column<int>(nullable: false),
                    Use = table.Column<int>(nullable: false)
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
                    ClassId = table.Column<int>(nullable: false),
                    ClassInfoId = table.Column<int>(nullable: false),
                    ClassWeekId = table.Column<int>(nullable: false),
                    ClassWeekTypeId = table.Column<int>(nullable: false),
                    Contents = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CwtParentId = table.Column<int>(nullable: false),
                    IsAudit = table.Column<int>(nullable: false),
                    OriginClassId = table.Column<int>(nullable: false),
                    OriginUniversityId = table.Column<int>(nullable: false),
                    RefId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UniversityId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
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
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Intro = table.Column<string>(nullable: true),
                    IsDel = table.Column<bool>(nullable: false),
                    IsHide = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RefId = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_University", x => x.Id);
                });
        }
    }
}
