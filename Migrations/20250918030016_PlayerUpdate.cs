using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectD_API.Migrations
{
    /// <inheritdoc />
    public partial class PlayerUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatPoint",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "StatPoint",
                table: "Players");
        }
    }
}
