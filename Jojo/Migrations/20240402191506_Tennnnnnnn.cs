using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jojo.Migrations
{
    /// <inheritdoc />
    public partial class Tennnnnnnn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImage",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "BackgroundImageContentType",
                table: "Users",
                newName: "BackgroundColor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackgroundColor",
                table: "Users",
                newName: "BackgroundImageContentType");

            migrationBuilder.AddColumn<byte[]>(
                name: "BackgroundImage",
                table: "Users",
                type: "bytea",
                nullable: true);
        }
    }
}
