using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class AnalyticsChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "store_analytics");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "store_analytics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "store_analytics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "store_analytics",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "store_analytics");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "store_analytics");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "store_analytics");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "store_analytics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
