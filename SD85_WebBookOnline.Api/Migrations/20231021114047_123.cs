using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class _123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1115a76-e783-428d-8567-801f9f90cdbb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcc9a2cd-3ffe-4f6a-be40-fd12bfddecf9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7e2f894e-39f6-4a61-a391-e8a52e685a80", "ba6b743f-c6fa-4c87-99e8-8aea0f7b7215", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "af4afd42-43c9-4795-9c19-b6f6d25c2066", "50675a57-f14f-4fad-8c2b-9921f5aa8cfd", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e2f894e-39f6-4a61-a391-e8a52e685a80");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af4afd42-43c9-4795-9c19-b6f6d25c2066");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b1115a76-e783-428d-8567-801f9f90cdbb", "2ed5b706-14b5-4a79-90e3-ac38c8703db9", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dcc9a2cd-3ffe-4f6a-be40-fd12bfddecf9", "f900e723-459d-4f32-a7bd-12074b735287", "User", "USER" });
        }
    }
}
