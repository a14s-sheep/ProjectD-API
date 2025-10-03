using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectD_API.Migrations
{
    /// <inheritdoc />
    public partial class PlayerSkillPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillPoint",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillPoint",
                table: "Players");
        }
    }
}
