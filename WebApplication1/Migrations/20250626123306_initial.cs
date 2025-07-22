using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48c6fc7c-9504-48be-b9ca-9ee0303a26fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "80fe7d21-9b9d-40b6-9ae4-61247020d0be");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "289f9973-ddbe-4f7a-bc02-e0a229e7ca90", null, "Admin", "ADMIN" },
                    { "d9a89e49-351c-4eec-8ab5-3f64973519c2", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "289f9973-ddbe-4f7a-bc02-e0a229e7ca90");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9a89e49-351c-4eec-8ab5-3f64973519c2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "48c6fc7c-9504-48be-b9ca-9ee0303a26fc", null, "Admin", "ADMIN" },
                    { "80fe7d21-9b9d-40b6-9ae4-61247020d0be", null, "User", "USER" }
                });
        }
    }
}
