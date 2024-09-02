using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_images_uploader_id",
                schema: "query",
                table: "images",
                column: "uploader_id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_images_users_uploader_id",
                schema: "query",
                table: "images",
                column: "uploader_id",
                principalSchema: "query",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_images_users_uploader_id",
                schema: "query",
                table: "images"
            );

            migrationBuilder.DropIndex(
                name: "ix_images_uploader_id",
                schema: "query",
                table: "images"
            );
        }
    }
}
