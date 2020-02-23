using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumApp.Migrations
{
    public partial class fixtypos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_BoardColumns_BoardColumnId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "BoarColumnId",
                table: "Stories");

            migrationBuilder.AlterColumn<int>(
                name: "BoardColumnId",
                table: "Stories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_BoardColumns_BoardColumnId",
                table: "Stories",
                column: "BoardColumnId",
                principalTable: "BoardColumns",
                principalColumn: "BoardColumnId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_BoardColumns_BoardColumnId",
                table: "Stories");

            migrationBuilder.AlterColumn<int>(
                name: "BoardColumnId",
                table: "Stories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BoarColumnId",
                table: "Stories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_BoardColumns_BoardColumnId",
                table: "Stories",
                column: "BoardColumnId",
                principalTable: "BoardColumns",
                principalColumn: "BoardColumnId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
