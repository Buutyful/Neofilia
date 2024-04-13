using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neofilia.Server.Migrations
{
    /// <inheritdoc />
    public partial class seeding3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "b29ac876-f02b-4ba1-91bd-e04f14f4170b", "nofilia_admin@libero.it", "NOFILIA_ADMIN@LIBERO.IT", "AQAAAAIAAYagAAAAEOeICvzvFdP4QBlFOwXfCNBllQnkBsX0KTZhe7wxLQ+93jIFuRo7pIIJ1e60+f128g==", "22d48c04-92e8-472a-9173-0140adfcdfc2", "nofilia_admin@libero.it" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "5c577b35-599f-4eef-8cca-3f3980b1e4af", "NOFILIA_ADMIN@LIBERO.IT", "ADMINUSER", "AQAAAAIAAYagAAAAECK90/n3u+9ztufUq/USRJkMhmyrqJNs1wG1Rr1KvP15uXMBoHVhHzJ/b9cjWTUS5A==", "b9d7b5d6-516f-4719-bbae-cf0aa6b299c7", "AdminUser" });
        }
    }
}
