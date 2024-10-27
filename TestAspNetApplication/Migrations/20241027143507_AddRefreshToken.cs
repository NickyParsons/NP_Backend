using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestAspNetApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a056de6a-63b9-4c33-bb02-212318eec503"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("29a715f7-2c5f-4dcd-87a8-696b6bf1be60"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2fc2aa93-cae2-48e7-8d96-32e8052b199a"));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpires",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("e02d3347-c23d-4a53-ba0b-199c33196d16"), "Пользователь", "User" },
                    { new Guid("f7db7b92-cc88-471b-aa6a-a1f381a0c703"), "Администратор", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "HashedPassword", "ImageUrl", "LastName", "PasswordResetToken", "RefreshToken", "RefreshTokenExpires", "ResetTokenExpires", "RoleId", "VerificationToken", "VerifiedAt" },
                values: new object[] { new Guid("bcfec4d0-dc43-4258-919b-1fdb6e15b738"), "admin@admin", null, "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", null, null, null, null, null, null, new Guid("f7db7b92-cc88-471b-aa6a-a1f381a0c703"), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e02d3347-c23d-4a53-ba0b-199c33196d16"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bcfec4d0-dc43-4258-919b-1fdb6e15b738"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f7db7b92-cc88-471b-aa6a-a1f381a0c703"));

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpires",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("2fc2aa93-cae2-48e7-8d96-32e8052b199a"), "Администратор", "Admin" },
                    { new Guid("a056de6a-63b9-4c33-bb02-212318eec503"), "Пользователь", "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "HashedPassword", "ImageUrl", "LastName", "PasswordResetToken", "ResetTokenExpires", "RoleId", "VerificationToken", "VerifiedAt" },
                values: new object[] { new Guid("29a715f7-2c5f-4dcd-87a8-696b6bf1be60"), "admin@admin", null, "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", null, null, null, null, new Guid("2fc2aa93-cae2-48e7-8d96-32e8052b199a"), null, null });
        }
    }
}
