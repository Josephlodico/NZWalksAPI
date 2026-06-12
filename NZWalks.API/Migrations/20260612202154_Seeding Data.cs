using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("45cc6145-3710-4a54-b3c6-06e969a1466b"),
                column: "Name",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("0b0b12bb-e9c0-4d0f-be28-1fe19b98d557"),
                column: "Name",
                value: "Bay Of Plenty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("45cc6145-3710-4a54-b3c6-06e969a1466b"),
                column: "Name",
                value: "");

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("0b0b12bb-e9c0-4d0f-be28-1fe19b98d557"),
                column: "Name",
                value: "Bay Of Pleanty");
        }
    }
}
