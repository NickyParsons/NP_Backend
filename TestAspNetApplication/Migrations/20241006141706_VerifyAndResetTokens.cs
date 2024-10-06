using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestAspNetApplication.Migrations
{
    /// <inheritdoc />
    public partial class VerifyAndResetTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6b069dc7-66ea-46f7-924b-46c0a1cc6a17"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ed1c98f1-dcdb-4665-b21f-ea9090bcd535"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("656068f8-6268-47f3-8851-c2c5dd03f044"));

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ResetTokenExpires",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VerifiedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("048c0991-10be-4c43-aaf8-09f7ee872628"), "Администратор", "Admin" },
                    { new Guid("8a6f663a-bafa-471b-8577-ba33fb4ea4b0"), "Пользователь", "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "HashedPassword", "ImageUrl", "LastName", "PasswordResetToken", "ResetTokenExpires", "RoleId", "VerificationToken", "VerifiedAt" },
                values: new object[] { new Guid("9504f5ca-e4a7-4fde-95bf-5245f563cbce"), "admin@admin", null, "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", null, null, null, null, new Guid("048c0991-10be-4c43-aaf8-09f7ee872628"), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8a6f663a-bafa-471b-8577-ba33fb4ea4b0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9504f5ca-e4a7-4fde-95bf-5245f563cbce"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("048c0991-10be-4c43-aaf8-09f7ee872628"));

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("656068f8-6268-47f3-8851-c2c5dd03f044"), "Администратор", "Admin" },
                    { new Guid("6b069dc7-66ea-46f7-924b-46c0a1cc6a17"), "Пользователь", "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "HashedPassword", "ImageUrl", "LastName", "RoleId" },
                values: new object[] { new Guid("ed1c98f1-dcdb-4665-b21f-ea9090bcd535"), "admin@admin", null, "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", null, null, new Guid("656068f8-6268-47f3-8851-c2c5dd03f044") });
        }
    }
}
