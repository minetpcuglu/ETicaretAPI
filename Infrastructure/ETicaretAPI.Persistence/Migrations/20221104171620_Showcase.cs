using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicaretAPI.Persistence.Migrations
{
    public partial class Showcase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Showcase",
                table: "Files",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenEndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showcase",
                table: "Files");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenEndDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
