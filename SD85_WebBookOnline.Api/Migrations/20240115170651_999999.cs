using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class _999999 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c93ab83-d24b-49b7-93f2-c9c91f57dfda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "930fe483-d370-4884-b16b-6ab921289596");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3953a7d-452d-46e2-bfbb-112eb04be224");

            migrationBuilder.AddColumn<decimal>(
                name: "GiaBan",
                table: "InputSlip",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GiaNhap",
                table: "BillItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1f5b3aa4-3702-4e1e-a3b1-c28ee6cdbaa6", "5e92e656-b607-4df0-9acb-e1f9812866a8", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b061414c-6cc8-47c8-ac89-09aeeb946be0", "07e9eae5-ae8c-4292-871a-fd932ee79b9b", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5aec01f-d73b-4c7b-9f1e-ecf0590f7bdc", "60b1021d-ef2e-4eec-b322-a8713390e552", "Empolyee", "EMPLOYEE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f5b3aa4-3702-4e1e-a3b1-c28ee6cdbaa6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b061414c-6cc8-47c8-ac89-09aeeb946be0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5aec01f-d73b-4c7b-9f1e-ecf0590f7bdc");

            migrationBuilder.DropColumn(
                name: "GiaBan",
                table: "InputSlip");

            migrationBuilder.DropColumn(
                name: "GiaNhap",
                table: "BillItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c93ab83-d24b-49b7-93f2-c9c91f57dfda", "72eff912-345c-4b24-94ed-0cf7fc133480", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "930fe483-d370-4884-b16b-6ab921289596", "4462bff4-d42a-4eff-893f-e460e4630a6e", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3953a7d-452d-46e2-bfbb-112eb04be224", "ae6c1093-0cb2-4a48-baa6-c0247f45e014", "Empolyee", "EMPLOYEE" });
        }
    }
}
