using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class getChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd31d39e-e1ba-4305-82fc-e4dabb5cac9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d692c559-0b7a-44ef-af7b-65b74d107b30");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4b30c600-7eea-4f8e-a1a5-4c0955551365", null, "Admin", "ADMIN" },
                    { "73bf27d0-69b4-485f-8a6f-5a0d706e8d25", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b30c600-7eea-4f8e-a1a5-4c0955551365");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73bf27d0-69b4-485f-8a6f-5a0d706e8d25");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cd31d39e-e1ba-4305-82fc-e4dabb5cac9f", null, "User", "USER" },
                    { "d692c559-0b7a-44ef-af7b-65b74d107b30", null, "Admin", "ADMIN" }
                });
        }
    }
}
