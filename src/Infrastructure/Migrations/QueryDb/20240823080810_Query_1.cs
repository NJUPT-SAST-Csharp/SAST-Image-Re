﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "query");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "query",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "users",
                schema: "query",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    username = table.Column<string>(type: "text", nullable: false),
                    biography = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true),
                    header_url = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "albums",
                schema: "query",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    collaborators = table.Column<long[]>(type: "bigint[]", nullable: false),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    is_removed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    updated_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    accessibility = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_albums", x => x.id);
                    table.ForeignKey(
                        name: "fk_albums_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "query",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_albums_users_author_id",
                        column: x => x.author_id,
                        principalSchema: "query",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "images",
                schema: "query",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    title = table.Column<string>(type: "text", nullable: false),
                    album_id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    tags = table.Column<long[]>(type: "bigint[]", nullable: false),
                    is_removed = table.Column<bool>(type: "boolean", nullable: false),
                    uploaded_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    accessibility = table.Column<int>(type: "integer", nullable: false),
                    album_model_id = table.Column<long>(type: "bigint", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_albums_album_id",
                        column: x => x.album_id,
                        principalSchema: "query",
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_images_albums_album_model_id",
                        column: x => x.album_model_id,
                        principalSchema: "query",
                        principalTable: "albums",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "fk_images_users_author_id",
                        column: x => x.author_id,
                        principalSchema: "query",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "subscribes",
                schema: "query",
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
                        principalSchema: "query",
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_subscribes_users_user",
                        column: x => x.user,
                        principalSchema: "query",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "likes",
                schema: "query",
                columns: table => new
                {
                    image = table.Column<long>(type: "bigint", nullable: false),
                    user = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_likes", x => new { x.image, x.user });
                    table.ForeignKey(
                        name: "fk_likes_images_image",
                        column: x => x.image,
                        principalSchema: "query",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_likes_users_user",
                        column: x => x.user,
                        principalSchema: "query",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_albums_author_id",
                schema: "query",
                table: "albums",
                column: "author_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_albums_category_id",
                schema: "query",
                table: "albums",
                column: "category_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_albums_title",
                schema: "query",
                table: "albums",
                column: "title",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_images_album_id",
                schema: "query",
                table: "images",
                column: "album_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_images_album_model_id",
                schema: "query",
                table: "images",
                column: "album_model_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_images_author_id",
                schema: "query",
                table: "images",
                column: "author_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_likes_user",
                schema: "query",
                table: "likes",
                column: "user"
            );

            migrationBuilder.CreateIndex(
                name: "ix_subscribes_user",
                schema: "query",
                table: "subscribes",
                column: "user"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "likes", schema: "query");

            migrationBuilder.DropTable(name: "subscribes", schema: "query");

            migrationBuilder.DropTable(name: "images", schema: "query");

            migrationBuilder.DropTable(name: "albums", schema: "query");

            migrationBuilder.DropTable(name: "categories", schema: "query");

            migrationBuilder.DropTable(name: "users", schema: "query");
        }
    }
}
