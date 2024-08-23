using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class domain_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "domain");

            migrationBuilder.CreateTable(
                name: "albums",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    collaborators = table.Column<long[]>(type: "bigint[]", nullable: false),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    is_removed = table.Column<bool>(type: "boolean", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    cover_id = table.Column<long>(type: "bigint", nullable: true),
                    cover_is_latest_image = table.Column<bool>(type: "boolean", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_albums", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "users",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    roles = table.Column<int[]>(type: "integer[]", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "images",
                schema: "domain",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    album_id = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_albums_album_id",
                        column: x => x.album_id,
                        principalSchema: "domain",
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

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
                    table.PrimaryKey("pk_subscribes", x => new { x.album, x.user });
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

            migrationBuilder.CreateTable(
                name: "likes",
                schema: "domain",
                columns: table => new
                {
                    image = table.Column<long>(type: "bigint", nullable: false),
                    user = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_likes", x => new { x.user, x.image });
                    table.ForeignKey(
                        name: "fk_likes_images_image",
                        column: x => x.image,
                        principalSchema: "domain",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_likes_users_user",
                        column: x => x.user,
                        principalSchema: "domain",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_albums_title",
                schema: "domain",
                table: "albums",
                column: "title",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_images_album_id",
                schema: "domain",
                table: "images",
                column: "album_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_likes_image",
                schema: "domain",
                table: "likes",
                column: "image"
            );

            migrationBuilder.CreateIndex(
                name: "ix_subscribes_user",
                schema: "domain",
                table: "subscribes",
                column: "user"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "categories", schema: "domain");

            migrationBuilder.DropTable(name: "likes", schema: "domain");

            migrationBuilder.DropTable(name: "subscribes", schema: "domain");

            migrationBuilder.DropTable(name: "tags", schema: "domain");

            migrationBuilder.DropTable(name: "images", schema: "domain");

            migrationBuilder.DropTable(name: "users", schema: "domain");

            migrationBuilder.DropTable(name: "albums", schema: "domain");
        }
    }
}
