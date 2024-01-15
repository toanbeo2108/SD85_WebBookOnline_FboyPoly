using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class _987789999 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e977c11-873f-4e67-b14b-d68d637a02c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a430f426-aebc-4c3a-820c-b5bc14193683");

            migrationBuilder.AddColumn<decimal>(
                name: "GiaNhap",
                table: "BillItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "InputSlipID",
                table: "BillItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "60eff3b2-6bb1-49e7-b173-5b07b3078ddb", "0289716d-a554-4be0-bb8d-85e7dd51e296", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f740561d-eb5b-4849-8dcc-f08f22f21d00", "cb14454b-7157-49d8-9cc4-eceaceda0241", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_InputSlipID",
                table: "BillItems",
                column: "InputSlipID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillItems_InputSlip_InputSlipID",
                table: "BillItems",
                column: "InputSlipID",
                principalTable: "InputSlip",
                principalColumn: "InputSlipID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillItems_InputSlip_InputSlipID",
                table: "BillItems");

            migrationBuilder.DropIndex(
                name: "IX_BillItems_InputSlipID",
                table: "BillItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60eff3b2-6bb1-49e7-b173-5b07b3078ddb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f740561d-eb5b-4849-8dcc-f08f22f21d00");

            migrationBuilder.DropColumn(
                name: "GiaNhap",
                table: "BillItems");

            migrationBuilder.DropColumn(
                name: "InputSlipID",
                table: "BillItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1e977c11-873f-4e67-b14b-d68d637a02c7", "0acd200b-225e-4541-afcf-0f46958cd5fc", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a430f426-aebc-4c3a-820c-b5bc14193683", "e2f15a7f-cac8-4006-b7c7-f5bb8c3cf318", "User", "USER" });
        }
    }
}
