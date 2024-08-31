using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long[]>(
                name: "collaborators",
                schema: "query",
                table: "images",
                type: "bigint[]",
                nullable: false,
                defaultValue: new long[0]);

            migrationBuilder.AddColumn<long>(
                name: "uploader_id",
                schema: "query",
                table: "images",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "collaborators",
                schema: "query",
                table: "images");

            migrationBuilder.DropColumn(
                name: "uploader_id",
                schema: "query",
                table: "images");
        }
    }
}
