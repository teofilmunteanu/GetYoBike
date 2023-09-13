using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetYoBike.Server.Migrations
{
    /// <inheritdoc />
    public partial class dates_changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentHoursDuration",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "RentStartDate",
                table: "Rents",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Rents",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Rents",
                newName: "RentStartDate");

            migrationBuilder.AddColumn<int>(
                name: "RentHoursDuration",
                table: "Rents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
