using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreImportSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchProperty",
                table: "store_import_settings");

            migrationBuilder.AddColumn<string>(
                name: "DbMatchProperty",
                table: "store_import_settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileMatchProperty",
                table: "store_import_settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "store_import_settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DbMatchProperty",
                table: "store_import_settings");

            migrationBuilder.DropColumn(
                name: "FileMatchProperty",
                table: "store_import_settings");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "store_import_settings");

            migrationBuilder.AddColumn<string>(
                name: "MatchProperty",
                table: "store_import_settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
