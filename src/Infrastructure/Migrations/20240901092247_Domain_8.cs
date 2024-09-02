using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Domain_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "uploader",
                schema: "domain",
                table: "images",
                newName: "uploader_id");

            migrationBuilder.CreateIndex(
                name: "ix_images_uploader_id",
                schema: "domain",
                table: "images",
                column: "uploader_id");

            migrationBuilder.CreateIndex(
                name: "ix_albums__author",
                schema: "domain",
                table: "albums",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_albums_users_author_id",
                schema: "domain",
                table: "albums",
                column: "author_id",
                principalSchema: "domain",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_images_users_uploader_id",
                schema: "domain",
                table: "images",
                column: "uploader_id",
                principalSchema: "domain",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_users_author_id",
                schema: "domain",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_images_users_uploader_id",
                schema: "domain",
                table: "images");

            migrationBuilder.DropIndex(
                name: "ix_images_uploader_id",
                schema: "domain",
                table: "images");

            migrationBuilder.DropIndex(
                name: "ix_albums__author",
                schema: "domain",
                table: "albums");

            migrationBuilder.RenameColumn(
                name: "uploader_id",
                schema: "domain",
                table: "images",
                newName: "uploader");
        }
    }
}
