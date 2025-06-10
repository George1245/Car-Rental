using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class secondmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_AspNetUsers_UserId1",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_UserId1",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Rents");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Rents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_UserId",
                table: "Rents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_UserId",
                table: "Rents");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Rents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Rents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rents_UserId1",
                table: "Rents",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_AspNetUsers_UserId1",
                table: "Rents",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
