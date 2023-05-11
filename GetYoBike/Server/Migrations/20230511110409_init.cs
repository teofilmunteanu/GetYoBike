using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetYoBike.Server.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BikeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BikeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bikes_BikeTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "BikeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    RenterUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RentedBikeId = table.Column<int>(type: "INTEGER", nullable: false),
                    RentStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RentHoursDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    CardNr = table.Column<string>(type: "TEXT", nullable: false),
                    CardExpMonth = table.Column<string>(type: "TEXT", nullable: false),
                    CardExpYear = table.Column<string>(type: "TEXT", nullable: false),
                    CardCVC = table.Column<string>(type: "TEXT", nullable: false),
                    PublicId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => new { x.RenterUserId, x.RentedBikeId });
                    table.ForeignKey(
                        name: "FK_Rents_Bikes_RentedBikeId",
                        column: x => x.RentedBikeId,
                        principalTable: "Bikes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rents_Users_RenterUserId",
                        column: x => x.RenterUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_TypeId",
                table: "Bikes",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_RentedBikeId",
                table: "Rents",
                column: "RentedBikeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BikeTypes");
        }
    }
}
