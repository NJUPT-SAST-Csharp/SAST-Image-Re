using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_images_albums_album_model_id",
                schema: "query",
                table: "images");

            migrationBuilder.DropIndex(
                name: "ix_images_album_model_id",
                schema: "query",
                table: "images");

            migrationBuilder.DropColumn(
                name: "album_model_id",
                schema: "query",
                table: "images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "album_model_id",
                schema: "query",
                table: "images",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_images_album_model_id",
                schema: "query",
                table: "images",
                column: "album_model_id");

            migrationBuilder.AddForeignKey(
                name: "fk_images_albums_album_model_id",
                schema: "query",
                table: "images",
                column: "album_model_id",
                principalSchema: "query",
                principalTable: "albums",
                principalColumn: "id");
        }
    }
}
