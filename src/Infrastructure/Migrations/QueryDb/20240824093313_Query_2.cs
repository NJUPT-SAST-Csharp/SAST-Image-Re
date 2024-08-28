using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "is_removed", schema: "query", table: "images");

            migrationBuilder.DropColumn(name: "is_removed", schema: "query", table: "albums");

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "query",
                table: "images",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "query",
                table: "albums",
                type: "timestamp with time zone",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "removed_at", schema: "query", table: "images");

            migrationBuilder.DropColumn(name: "removed_at", schema: "query", table: "albums");

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "query",
                table: "images",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "query",
                table: "albums",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );
        }
    }
}
