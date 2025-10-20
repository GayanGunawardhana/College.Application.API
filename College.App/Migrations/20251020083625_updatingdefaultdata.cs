using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace College.App.Migrations
{
    /// <inheritdoc />
    public partial class updatingdefaultdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "Email", "StudentName" },
                values: new object[,]
                {
                    { 1, "Colombo", new DateTime(1998, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "gayan@gmail.com", "Gayan Gunawardhana" },
                    { 2, "Kandy", new DateTime(1997, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "kamal@gmail.com", "Kamal Perera" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
