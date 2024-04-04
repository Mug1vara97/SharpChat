using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jojo.Migrations
{
    /// <inheritdoc />
    public partial class Onenт : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_NewsFeedItems_NewsFeedItemId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_NewsFeedItemId",
                table: "Comments",
                newName: "IX_Comments_NewsFeedItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_NewsFeedItems_NewsFeedItemId",
                table: "Comments",
                column: "NewsFeedItemId",
                principalTable: "NewsFeedItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_NewsFeedItems_NewsFeedItemId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_NewsFeedItemId",
                table: "Comment",
                newName: "IX_Comment_NewsFeedItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_NewsFeedItems_NewsFeedItemId",
                table: "Comment",
                column: "NewsFeedItemId",
                principalTable: "NewsFeedItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
