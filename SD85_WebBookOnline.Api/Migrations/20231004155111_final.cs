using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD85_WebBookOnline.Api.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookDetails_Author_AuthorID",
                table: "BookDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Author",
                table: "Author");

            migrationBuilder.RenameTable(
                name: "Author",
                newName: "Authors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                table: "Authors",
                column: "AuthorID");

            migrationBuilder.CreateTable(
                name: "PostBanner",
                columns: table => new
                {
                    PostID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBanner", x => x.PostID);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "443c3e7c-0467-443d-9caa-b65f4dd13c0a", "d07207e0-0b7c-4d48-89f1-d8d60cfc2aab", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d46f8c38-4726-479d-bc89-5d77d715d925", "1a2f5c0c-4935-4f21-b3f9-35d1c1aae12f", "User", "USER" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookDetails_Authors_AuthorID",
                table: "BookDetails",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookDetails_Authors_AuthorID",
                table: "BookDetails");

            migrationBuilder.DropTable(
                name: "PostBanner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                table: "Authors");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "443c3e7c-0467-443d-9caa-b65f4dd13c0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d46f8c38-4726-479d-bc89-5d77d715d925");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "Author");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Author",
                table: "Author",
                column: "AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookDetails_Author_AuthorID",
                table: "BookDetails",
                column: "AuthorID",
                principalTable: "Author",
                principalColumn: "AuthorID");
        }
    }
}
