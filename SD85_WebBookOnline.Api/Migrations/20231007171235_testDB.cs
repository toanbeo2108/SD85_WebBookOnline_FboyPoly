using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class testDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "443c3e7c-0467-443d-9caa-b65f4dd13c0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d46f8c38-4726-479d-bc89-5d77d715d925");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Languges",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Coupon",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1e379e57-8562-438f-8ce0-286f9ba266bd", "d1b4dd81-a1c8-480e-a961-090200938ed8", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ca1a154c-19a7-4e5d-8f4c-4142342e0765", "f5bbeead-0078-4440-80cd-2fc6b445438a", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e379e57-8562-438f-8ce0-286f9ba266bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca1a154c-19a7-4e5d-8f4c-4142342e0765");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Languges",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Coupon",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "443c3e7c-0467-443d-9caa-b65f4dd13c0a", "d07207e0-0b7c-4d48-89f1-d8d60cfc2aab", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d46f8c38-4726-479d-bc89-5d77d715d925", "1a2f5c0c-4935-4f21-b3f9-35d1c1aae12f", "User", "USER" });
        }
    }
}
