using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class FixHardDifficultyGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("f808ddcd-b5e5-4d80-b732-1ca523e48434"));

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("c9b7c5e8-3a1d-4b5f-9e2a-8d6f4c7b3e1a"), "Hard" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("c9b7c5e8-3a1d-4b5f-9e2a-8d6f4c7b3e1a"));

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("f808ddcd-b5e5-4d80-b732-1ca523e48434"), "Hard" });
        }
    }
}
