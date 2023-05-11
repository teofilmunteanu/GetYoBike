using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetYoBike.Server.Migrations
{
    /// <inheritdoc />
    public partial class PriceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDiscounted",
                table: "Rents",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Rents",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDiscounted",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rents");
        }
    }
}
