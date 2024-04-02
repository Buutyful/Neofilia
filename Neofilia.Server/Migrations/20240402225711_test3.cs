using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neofilia.Server.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Locals",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Locals",
                newName: "Address_PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "CivilNumber",
                table: "Locals",
                newName: "Address_CivilNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Locals",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Address_PhoneNumber",
                table: "Locals",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Address_CivilNumber",
                table: "Locals",
                newName: "CivilNumber");
        }
    }
}
