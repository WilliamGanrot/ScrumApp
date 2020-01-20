using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumApp.Migrations
{
    public partial class add_UserNameSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNameSlug",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNameSlug",
                table: "AspNetUsers");
        }
    }
}
