using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class q : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

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
                    Difficulty = table.Column<string>(nullable: true),
                    Professor = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true)
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
                    ClassId = table.Column<int>(nullable: false),
                    TotalGrade = table.Column<int>(nullable: false),
                    Evaluate = table.Column<string>(nullable: true),
                    Use = table.Column<int>(nullable: false),
                    NoUse = table.Column<int>(nullable: false),
                    PageViews = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassInfo", x => x.Id);
                });


            migrationBuilder.CreateTable(
                name: "ClassWeek",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ClassInfoId = table.Column<int>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    Grade = table.Column<int>(nullable: false)
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
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ClassWeekId = table.Column<int>(nullable: false),
                    ClassWeekTypeId = table.Column<int>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    Grade = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassWeekType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    QQ = table.Column<string>(nullable: true),
                    Tel = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    Contents = table.Column<string>(nullable: true),
                    ClassInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    StartId = table.Column<int>(nullable: false),
                    EndId = table.Column<int>(nullable: false),
                    Table = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    SendId = table.Column<int>(nullable: false),
                    ReceiveId = table.Column<int>(nullable: false),
                    ContentsUrl = table.Column<string>(nullable: true),
                    IsRead = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    OrderNo = table.Column<string>(nullable: true),
                    PayTime = table.Column<DateTime>(nullable: false),
                    PayType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Price = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Memo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "University",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Intro = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_University", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    QQ = table.Column<string>(nullable: true),
                    Tel = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    isTerminated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "ClassInfo");


            migrationBuilder.DropTable(
                name: "ClassWeek");

            migrationBuilder.DropTable(
                name: "ClassWeekType");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "ImportRecords");

            migrationBuilder.DropTable(
                name: "Notice");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "University");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
