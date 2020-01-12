using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumApp.Migrations
{
    public partial class addauthortouserprojectmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Projects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
