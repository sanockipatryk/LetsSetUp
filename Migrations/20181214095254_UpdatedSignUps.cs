using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsSetUp.Migrations
{
    public partial class UpdatedSignUps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUps_AspNetUsers_AppUserId",
                table: "SignUps");

            migrationBuilder.DropIndex(
                name: "IX_SignUps_AppUserId",
                table: "SignUps");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "SignUps");

            migrationBuilder.CreateIndex(
                name: "IX_SignUps_UserId",
                table: "SignUps",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUps_AspNetUsers_UserId",
                table: "SignUps",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUps_AspNetUsers_UserId",
                table: "SignUps");

            migrationBuilder.DropIndex(
                name: "IX_SignUps_UserId",
                table: "SignUps");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "SignUps",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SignUps_AppUserId",
                table: "SignUps",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUps_AspNetUsers_AppUserId",
                table: "SignUps",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
