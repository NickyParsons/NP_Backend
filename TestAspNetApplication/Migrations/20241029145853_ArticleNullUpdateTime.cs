using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestAspNetApplication.Migrations
{
    /// <inheritdoc />
    public partial class ArticleNullUpdateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Articles",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("44abda67-2753-4dd0-a5c2-9559314b556a"), "Пользователь", "User" },
                    { new Guid("59cd2ea8-ec7c-4a16-b469-23bc0f1ffe81"), "Администратор", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "HashedPassword", "ImageUrl", "LastName", "PasswordResetToken", "RefreshToken", "RefreshTokenExpires", "ResetTokenExpires", "RoleId", "VerificationToken", "VerifiedAt" },
                values: new object[] { new Guid("714e4522-ce6c-4472-9a2c-0c8363bc3cb0"), "admin@admin", null, "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", null, null, null, null, null, null, new Guid("59cd2ea8-ec7c-4a16-b469-23bc0f1ffe81"), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("44abda67-2753-4dd0-a5c2-9559314b556a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("714e4522-ce6c-4472-9a2c-0c8363bc3cb0"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("59cd2ea8-ec7c-4a16-b469-23bc0f1ffe81"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Articles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

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
    }
}
