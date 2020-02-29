using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumApp.Migrations
{
    public partial class storysorting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StorySorting",
                table: "Stories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorySorting",
                table: "Stories");
        }
    }
}
