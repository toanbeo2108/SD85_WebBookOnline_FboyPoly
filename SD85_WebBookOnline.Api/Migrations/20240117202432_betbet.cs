using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class betbet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9f1d4a9-7262-48ef-b2ba-9cb1c5df160a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b924f90c-4ba1-4c0a-ba20-03414be5f013");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc21b820-2122-40a7-9896-0a22f431847a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8579f0b6-c6e5-49ff-a0cd-74d47926e1f0", "73fd8342-2124-4201-b8f4-4487e7a818cb", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c26d6a61-b923-47fd-9e19-cae8e1095208", "7ccb0a91-358a-4e81-a64f-e46d9590f01b", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ddad87d6-4557-4c40-ae48-6bb41857f7ee", "9e094a91-27ca-4e62-b67c-e884a1cabada", "Employee", "EMPLOYEE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8579f0b6-c6e5-49ff-a0cd-74d47926e1f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c26d6a61-b923-47fd-9e19-cae8e1095208");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddad87d6-4557-4c40-ae48-6bb41857f7ee");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a9f1d4a9-7262-48ef-b2ba-9cb1c5df160a", "be565570-557e-4d9b-a7a3-8ddbb4305820", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b924f90c-4ba1-4c0a-ba20-03414be5f013", "69b0c1b9-ec61-4500-a601-0fa0ef21fdb2", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bc21b820-2122-40a7-9896-0a22f431847a", "163c25fe-327c-489e-8491-5c7c491e706e", "Admin", "ADMIN" });
        }
    }
}
