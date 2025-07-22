using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
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
                    { "0276c6c9-9acb-4a36-9aa0-48ca2670cfa2", null, "Admin", "ADMIN" },
                    { "46d4f109-9acf-411f-8c06-86075aa8e005", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0276c6c9-9acb-4a36-9aa0-48ca2670cfa2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46d4f109-9acf-411f-8c06-86075aa8e005");

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
