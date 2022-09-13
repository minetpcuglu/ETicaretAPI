using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicaretAPI.Persistence.Migrations
{
    public partial class StorageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StorageType",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageType",
                table: "Files");
        }
    }
}
