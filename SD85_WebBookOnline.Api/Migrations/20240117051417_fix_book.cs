using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class fix_book : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "Book",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3d6c1e43-bfa8-493a-863a-3345d6ec89cb", "b4bde7b0-5b4d-448a-9162-31238952aafe", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cda6b623-324b-4c8d-b34e-316edfb469f1", "049d0fdd-5f85-4603-bed2-34d009b1a69c", "Empolyee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e36c9b96-b7b2-4ada-b481-e584d47cb285", "186de966-a743-448c-92e8-4fc63997693c", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d6c1e43-bfa8-493a-863a-3345d6ec89cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cda6b623-324b-4c8d-b34e-316edfb469f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e36c9b96-b7b2-4ada-b481-e584d47cb285");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Book",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
