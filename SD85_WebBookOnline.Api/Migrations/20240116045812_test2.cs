using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "222c62eb-19b3-4740-8ad3-244e5bc48641", "9dbc38e9-b07f-4a7f-b72c-3814e4fe7e2f", "Empolyee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8c64b477-7de1-4d10-8e66-028263d0f089", "b2d4bd81-cee0-4d92-b7af-22355fcb2249", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c39f0748-159f-4ced-a418-b47e8e4bfa58", "2ff5bdb6-8e4a-4df7-a112-59289082832c", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "222c62eb-19b3-4740-8ad3-244e5bc48641");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c64b477-7de1-4d10-8e66-028263d0f089");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c39f0748-159f-4ced-a418-b47e8e4bfa58");

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
    }
}
