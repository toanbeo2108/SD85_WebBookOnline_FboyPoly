using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class _33663 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e379e57-8562-438f-8ce0-286f9ba266bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca1a154c-19a7-4e5d-8f4c-4142342e0765");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Combo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b1115a76-e783-428d-8567-801f9f90cdbb", "2ed5b706-14b5-4a79-90e3-ac38c8703db9", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dcc9a2cd-3ffe-4f6a-be40-fd12bfddecf9", "f900e723-459d-4f32-a7bd-12074b735287", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1115a76-e783-428d-8567-801f9f90cdbb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcc9a2cd-3ffe-4f6a-be40-fd12bfddecf9");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Combo");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1e379e57-8562-438f-8ce0-286f9ba266bd", "d1b4dd81-a1c8-480e-a961-090200938ed8", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ca1a154c-19a7-4e5d-8f4c-4142342e0765", "f5bbeead-0078-4440-80cd-2fc6b445438a", "Admin", "ADMIN" });
        }
    }
}
