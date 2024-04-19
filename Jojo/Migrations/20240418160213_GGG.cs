using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jojo.Migrations
{
    /// <inheritdoc />
    public partial class GGG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "FriendRequests");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "FriendRequests");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverUsername",
                table: "FriendRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderUsername",
                table: "FriendRequests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverUsername",
                table: "FriendRequests");

            migrationBuilder.DropColumn(
                name: "SenderUsername",
                table: "FriendRequests");

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "FriendRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "FriendRequests",
                type: "text",
                nullable: true);
        }
    }
}
