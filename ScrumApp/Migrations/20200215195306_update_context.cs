using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumApp.Migrations
{
    public partial class update_context : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardColumn_Boards_BoardId",
                table: "BoardColumn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardColumn",
                table: "BoardColumn");

            migrationBuilder.RenameTable(
                name: "BoardColumn",
                newName: "BoardColumns");

            migrationBuilder.RenameIndex(
                name: "IX_BoardColumn_BoardId",
                table: "BoardColumns",
                newName: "IX_BoardColumns_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardColumns",
                table: "BoardColumns",
                column: "BoardColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardColumns_Boards_BoardId",
                table: "BoardColumns",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardColumns_Boards_BoardId",
                table: "BoardColumns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardColumns",
                table: "BoardColumns");

            migrationBuilder.RenameTable(
                name: "BoardColumns",
                newName: "BoardColumn");

            migrationBuilder.RenameIndex(
                name: "IX_BoardColumns_BoardId",
                table: "BoardColumn",
                newName: "IX_BoardColumn_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardColumn",
                table: "BoardColumn",
                column: "BoardColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardColumn_Boards_BoardId",
                table: "BoardColumn",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
