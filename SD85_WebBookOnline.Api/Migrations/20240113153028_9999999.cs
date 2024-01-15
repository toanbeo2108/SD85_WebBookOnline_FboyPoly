using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class _9999999 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73c2f3da-89ed-4c17-9f1a-a71fd125cbeb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfc72319-4755-4a5f-a29b-376ec2f45b37");

            migrationBuilder.AddColumn<decimal>(
                name: "GiaBan",
                table: "InputSlip",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1e977c11-873f-4e67-b14b-d68d637a02c7", "0acd200b-225e-4541-afcf-0f46958cd5fc", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a430f426-aebc-4c3a-820c-b5bc14193683", "e2f15a7f-cac8-4006-b7c7-f5bb8c3cf318", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e977c11-873f-4e67-b14b-d68d637a02c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a430f426-aebc-4c3a-820c-b5bc14193683");

            migrationBuilder.DropColumn(
                name: "GiaBan",
                table: "InputSlip");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "73c2f3da-89ed-4c17-9f1a-a71fd125cbeb", "bfa8a29a-6923-4fe5-ae57-ae37ae012649", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bfc72319-4755-4a5f-a29b-376ec2f45b37", "bd7894a7-7aa5-48e5-b115-1b44061f7366", "Admin", "ADMIN" });
        }
    }
}
