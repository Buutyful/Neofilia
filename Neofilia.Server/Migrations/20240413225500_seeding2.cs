using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neofilia.Server.Migrations
{
    /// <inheritdoc />
    public partial class seeding2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5c577b35-599f-4eef-8cca-3f3980b1e4af", "AQAAAAIAAYagAAAAECK90/n3u+9ztufUq/USRJkMhmyrqJNs1wG1Rr1KvP15uXMBoHVhHzJ/b9cjWTUS5A==", "b9d7b5d6-516f-4719-bbae-cf0aa6b299c7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eee881bd-ee2c-4687-8a45-b3e49063049f", "AQAAAAIAAYagAAAAEPUVGYcEfYeRZSfVXoVUHtSrwxfSZaRV6E/MYG5EKj/sYu/YH7itrn25vYOffKETyQ==", "231cd0c5-3f13-4a35-88a2-540de4f13a8a" });
        }
    }
}
