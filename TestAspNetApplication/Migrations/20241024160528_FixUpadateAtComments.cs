using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestAspNetApplication.Migrations
{
    /// <inheritdoc />
    public partial class FixUpadateAtComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("869ebae4-748a-46a7-ab9c-8628fedcc086"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5e08c879-b535-4236-9a43-20785d4c711c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("64c3e3d5-5451-4ef1-b78b-dd4d5b9b2d88"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Comments",
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
                    { new Guid("64c3e3d5-5451-4ef1-b78b-dd4d5b9b2d88"), "Администратор", "Admin" },
                    { new Guid("869ebae4-748a-46a7-ab9c-8628fedcc086"), "Пользователь", "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "HashedPassword", "ImageUrl", "LastName", "PasswordResetToken", "ResetTokenExpires", "RoleId", "VerificationToken", "VerifiedAt" },
                values: new object[] { new Guid("5e08c879-b535-4236-9a43-20785d4c711c"), "admin@admin", null, "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", null, null, null, null, new Guid("64c3e3d5-5451-4ef1-b78b-dd4d5b9b2d88"), null, null });
        }
    }
}
