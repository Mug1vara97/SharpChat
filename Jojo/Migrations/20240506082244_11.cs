using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jojo.Migrations
{
    /// <inheritdoc />
    public partial class _11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SnakeStatistics_UserId",
                table: "SnakeStatistics",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SnakeStatistics_Users_UserId",
                table: "SnakeStatistics",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnakeStatistics_Users_UserId",
                table: "SnakeStatistics");

            migrationBuilder.DropIndex(
                name: "IX_SnakeStatistics_UserId",
                table: "SnakeStatistics");
        }
    }
}
