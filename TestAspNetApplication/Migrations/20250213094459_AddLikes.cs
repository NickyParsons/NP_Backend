using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestAspNetApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ArticleUser",
                columns: table => new
                {
                    LikedArticlesId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedById = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleUser", x => new { x.LikedArticlesId, x.LikedById });
                    table.ForeignKey(
                        name: "FK_ArticleUser_Articles_LikedArticlesId",
                        column: x => x.LikedArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleUser_Users_LikedById",
                        column: x => x.LikedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentUser",
                columns: table => new
                {
                    LikedById = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedCommentsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUser", x => new { x.LikedById, x.LikedCommentsId });
                    table.ForeignKey(
                        name: "FK_CommentUser_Comments_LikedCommentsId",
                        column: x => x.LikedCommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentUser_Users_LikedById",
                        column: x => x.LikedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("22852fda-63c6-4bd8-ba6a-6013591a9ae8"), "Пользователь", "User" },
                    { new Guid("bf1cd95c-0e13-4469-a2fb-e0910894b25f"), "Администратор", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "HashedPassword", "ImageUrl", "LastName", "PasswordResetToken", "RefreshToken", "RefreshTokenExpires", "ResetTokenExpires", "RoleId", "VerificationToken", "VerifiedAt" },
                values: new object[] { new Guid("8f9a9dbb-e4e0-4a7b-a77d-296fe3037528"), "admin@admin", null, "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", null, null, null, null, null, null, new Guid("bf1cd95c-0e13-4469-a2fb-e0910894b25f"), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleUser_LikedById",
                table: "ArticleUser",
                column: "LikedById");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUser_LikedCommentsId",
                table: "CommentUser",
                column: "LikedCommentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleUser");

            migrationBuilder.DropTable(
                name: "CommentUser");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("22852fda-63c6-4bd8-ba6a-6013591a9ae8"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8f9a9dbb-e4e0-4a7b-a77d-296fe3037528"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bf1cd95c-0e13-4469-a2fb-e0910894b25f"));

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
    }
}
