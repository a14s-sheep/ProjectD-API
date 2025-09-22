using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectD_API.Migrations
{
    /// <inheritdoc />
    public partial class PlayerStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Agility",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "Players",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Power",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Vitality",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agility",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Vitality",
                table: "Players");
        }
    }
}
