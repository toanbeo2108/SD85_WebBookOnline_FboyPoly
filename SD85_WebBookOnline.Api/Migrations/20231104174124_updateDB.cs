using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class updateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "648d553e-5e13-4144-8c87-5344b2014af6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9d98bf9-47f2-478e-b3e7-6b7e4ced6b65");

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                table: "Book",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Book",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9d41b08f-1cae-407f-9557-240413c91125", "342f0de6-a234-482d-aad1-2abffe598fc7", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f24ea11f-6e7c-4b31-8e6c-45f95f715766", "341de6ee-91e9-4b4d-bdda-bbd8aea764fe", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d41b08f-1cae-407f-9557-240413c91125");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f24ea11f-6e7c-4b31-8e6c-45f95f715766");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Book");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "648d553e-5e13-4144-8c87-5344b2014af6", "49ea3dc9-f776-4657-a2b0-30b4a4a65658", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f9d98bf9-47f2-478e-b3e7-6b7e4ced6b65", "064b47c8-121f-4449-b7b4-1d3718b41cd0", "User", "USER" });
        }
    }
}
