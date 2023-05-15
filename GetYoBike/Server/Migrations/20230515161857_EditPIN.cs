using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetYoBike.Server.Migrations
{
    /// <inheritdoc />
    public partial class EditPIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "Rents",
                newName: "EditPIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EditPIN",
                table: "Rents",
                newName: "PublicId");
        }
    }
}
