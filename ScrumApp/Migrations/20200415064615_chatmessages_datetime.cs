using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumApp.Migrations
{
    public partial class chatmessages_datetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeSent",
                table: "ChatMessages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSent",
                table: "ChatMessages");
        }
    }
}
