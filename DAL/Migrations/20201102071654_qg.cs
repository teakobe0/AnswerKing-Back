using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class qg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassInfoId",
                table: "Comment",
                newName: "TypeId");

            migrationBuilder.AddColumn<string>(
                name: "ContentsId",
                table: "Notice",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Inviterid",
                table: "Client",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginIP",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Client",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "LoginIP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginIP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PId = table.Column<string>(nullable: true),
                    IsDel = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginIP");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropColumn(
                name: "ContentsId",
                table: "Notice");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "LoginIP",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Client");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Comment",
                newName: "ClassInfoId");

            migrationBuilder.AlterColumn<int>(
                name: "Inviterid",
                table: "Client",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
