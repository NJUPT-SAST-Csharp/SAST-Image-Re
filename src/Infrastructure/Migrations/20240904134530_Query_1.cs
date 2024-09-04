using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Query_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long[]>(
                name: "tags",
                schema: "query",
                table: "images",
                type: "bigint[]",
                nullable: false,
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "query",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                schema: "query",
                table: "tags",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tags",
                schema: "query");

            migrationBuilder.AlterColumn<string[]>(
                name: "tags",
                schema: "query",
                table: "images",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(long[]),
                oldType: "bigint[]");
        }
    }
}
