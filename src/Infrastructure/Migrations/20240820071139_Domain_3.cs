using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Domain_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "subscribers", schema: "domain");

            migrationBuilder.CreateTable(
                name: "subscribes",
                schema: "domain",
                columns: table => new
                {
                    album = table.Column<long>(type: "bigint", nullable: false),
                    user = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscribes", x => new { x.user, x.album });
                    table.ForeignKey(
                        name: "fk_subscribes_albums_album",
                        column: x => x.album,
                        principalSchema: "domain",
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_subscribes_users_user",
                        column: x => x.user,
                        principalSchema: "domain",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_subscribes_album",
                schema: "domain",
                table: "subscribes",
                column: "album"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "subscribes", schema: "domain");

            migrationBuilder.CreateTable(
                name: "subscribers",
                schema: "domain",
                columns: table => new
                {
                    user = table.Column<long>(type: "bigint", nullable: false),
                    album = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscribers", x => new { x.user, x.album });
                    table.ForeignKey(
                        name: "fk_subscribers_albums_album",
                        column: x => x.album,
                        principalSchema: "domain",
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_subscribers_users_user",
                        column: x => x.user,
                        principalSchema: "domain",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_subscribers_album",
                schema: "domain",
                table: "subscribers",
                column: "album"
            );
        }
    }
}
