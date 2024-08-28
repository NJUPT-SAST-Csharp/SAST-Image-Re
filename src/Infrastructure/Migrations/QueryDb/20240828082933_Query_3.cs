using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "ix_users_username",
                schema: "query",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                schema: "query",
                table: "categories",
                column: "name",
                unique: true);

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

            migrationBuilder.DropIndex(
                name: "ix_users_username",
                schema: "query",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_categories_name",
                schema: "query",
                table: "categories");
        }
    }
}
