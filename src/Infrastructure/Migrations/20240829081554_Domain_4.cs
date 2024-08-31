using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Domain_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_albums_id",
                schema: "domain",
                table: "albums");

            migrationBuilder.AlterColumn<long>(
                name: "cover_id",
                schema: "domain",
                table: "albums",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "accessibility",
                schema: "domain",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                schema: "domain",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_username",
                schema: "domain",
                table: "users");

            migrationBuilder.DropColumn(
                name: "accessibility",
                schema: "domain",
                table: "albums");

            migrationBuilder.AlterColumn<long>(
                name: "cover_id",
                schema: "domain",
                table: "albums",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_albums_id",
                schema: "domain",
                table: "albums",
                column: "cover_id",
                principalSchema: "domain",
                principalTable: "albums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
