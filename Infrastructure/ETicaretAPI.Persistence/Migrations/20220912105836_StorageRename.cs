using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicaretAPI.Persistence.Migrations
{
    public partial class StorageRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorageType",
                table: "Files",
                newName: "Storage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Storage",
                table: "Files",
                newName: "StorageType");
        }
    }
}
