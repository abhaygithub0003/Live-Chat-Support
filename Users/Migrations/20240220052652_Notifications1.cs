using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Migrations
{
    /// <inheritdoc />
    public partial class Notifications1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_GroupUsers_GroupUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_GroupUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "GroupUserId",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupUserId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_GroupUserId",
                table: "Notifications",
                column: "GroupUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_GroupUsers_GroupUserId",
                table: "Notifications",
                column: "GroupUserId",
                principalTable: "GroupUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
