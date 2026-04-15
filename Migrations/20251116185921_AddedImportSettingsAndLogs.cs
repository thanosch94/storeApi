using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedImportSettingsAndLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FolderToSave",
                table: "store_import_settings",
                newName: "Folder");

            migrationBuilder.AddColumn<int>(
                name: "ImportType",
                table: "store_import_settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MatchProperty",
                table: "store_import_settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UpdateExistingEntities",
                table: "store_import_settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "store_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    LogOrigin = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSeeded = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAdded = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserUpdated = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store_logs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "store_logs");

            migrationBuilder.DropColumn(
                name: "ImportType",
                table: "store_import_settings");

            migrationBuilder.DropColumn(
                name: "MatchProperty",
                table: "store_import_settings");

            migrationBuilder.DropColumn(
                name: "UpdateExistingEntities",
                table: "store_import_settings");

            migrationBuilder.RenameColumn(
                name: "Folder",
                table: "store_import_settings",
                newName: "FolderToSave");
        }
    }
}
