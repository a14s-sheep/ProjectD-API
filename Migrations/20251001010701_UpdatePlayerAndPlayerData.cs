using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectD_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlayerAndPlayerData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Armor",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ArmorReduction",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AttackSpeed",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CritChance",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CritPower",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Damage",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HealthRegen",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MaxHealth",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MoveSpeed",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Armor",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ArmorReduction",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "AttackSpeed",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CritChance",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CritPower",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Damage",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "HealthRegen",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MaxHealth",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MoveSpeed",
                table: "Players");
        }
    }
}
