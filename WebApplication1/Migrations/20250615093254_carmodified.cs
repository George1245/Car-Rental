using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class carmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81d3d289-be47-457e-8a6c-83c8abfb7a65");

            migrationBuilder.DropColumn(
                name: "CostPerHour",
                table: "Rents");

            migrationBuilder.AddColumn<double>(
                name: "totalCost",
                table: "Rents",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "costPerHour",
                table: "Cars",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ad6ca9f5-a6f9-4679-8f27-003f40b91fba", null, "User", "USER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad6ca9f5-a6f9-4679-8f27-003f40b91fba");

            migrationBuilder.DropColumn(
                name: "totalCost",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "costPerHour",
                table: "Cars");

            migrationBuilder.AddColumn<float>(
                name: "CostPerHour",
                table: "Rents",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81d3d289-be47-457e-8a6c-83c8abfb7a65", null, "User", "USER" });
        }
    }
}
