using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class crateDbHiepdz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d41b08f-1cae-407f-9557-240413c91125");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f24ea11f-6e7c-4b31-8e6c-45f95f715766");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Combo",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "CartItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "68afffd5-6195-4974-a4f9-c60d77293905", "79969aac-f673-4d7b-8ac6-e18ea845048d", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9d04231e-cead-40d8-9567-5a441710722a", "ecf0d5ec-2d64-468c-b647-788c37fd10de", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68afffd5-6195-4974-a4f9-c60d77293905");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d04231e-cead-40d8-9567-5a441710722a");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Combo");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "CartItems");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9d41b08f-1cae-407f-9557-240413c91125", "342f0de6-a234-482d-aad1-2abffe598fc7", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f24ea11f-6e7c-4b31-8e6c-45f95f715766", "341de6ee-91e9-4b4d-bdda-bbd8aea764fe", "Admin", "ADMIN" });
        }
    }
}
