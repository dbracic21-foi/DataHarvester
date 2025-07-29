using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataHarvester.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: new Guid("b9993ce4-3ca3-4309-be3f-cd8476c93547"));

            migrationBuilder.DeleteData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: new Guid("df94efb2-d3d4-44ae-a8ac-64a1d6e3ce68"));

            migrationBuilder.InsertData(
                table: "DataSources",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Crypto API", "crypto" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Weather API", "weather" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "DataSources",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.InsertData(
                table: "DataSources",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("b9993ce4-3ca3-4309-be3f-cd8476c93547"), "Weather API", "weather" },
                    { new Guid("df94efb2-d3d4-44ae-a8ac-64a1d6e3ce68"), "Crypto API", "crypto" }
                });
        }
    }
}
