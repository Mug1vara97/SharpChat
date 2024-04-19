using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jojo.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatListViewModelUsername",
                table: "UnreadMessageCounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatListViewModelUsername",
                table: "Chats",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatListViewModels",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatListViewModels", x => x.Username);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnreadMessageCounts_ChatListViewModelUsername",
                table: "UnreadMessageCounts",
                column: "ChatListViewModelUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ChatListViewModelUsername",
                table: "Chats",
                column: "ChatListViewModelUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_ChatListViewModels_ChatListViewModelUsername",
                table: "Chats",
                column: "ChatListViewModelUsername",
                principalTable: "ChatListViewModels",
                principalColumn: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_UnreadMessageCounts_ChatListViewModels_ChatListViewModelUse~",
                table: "UnreadMessageCounts",
                column: "ChatListViewModelUsername",
                principalTable: "ChatListViewModels",
                principalColumn: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_ChatListViewModels_ChatListViewModelUsername",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_UnreadMessageCounts_ChatListViewModels_ChatListViewModelUse~",
                table: "UnreadMessageCounts");

            migrationBuilder.DropTable(
                name: "ChatListViewModels");

            migrationBuilder.DropIndex(
                name: "IX_UnreadMessageCounts_ChatListViewModelUsername",
                table: "UnreadMessageCounts");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ChatListViewModelUsername",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatListViewModelUsername",
                table: "UnreadMessageCounts");

            migrationBuilder.DropColumn(
                name: "ChatListViewModelUsername",
                table: "Chats");
        }
    }
}
