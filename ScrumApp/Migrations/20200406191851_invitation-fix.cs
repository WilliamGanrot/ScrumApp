using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumApp.Migrations
{
    public partial class invitationfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ProjectInvitations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ProjectInvitations");
        }
    }
}
