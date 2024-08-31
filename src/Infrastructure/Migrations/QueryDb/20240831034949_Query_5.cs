using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_archived",
                schema: "query",
                table: "albums");

            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "query",
                table: "images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                schema: "query",
                table: "albums",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "query",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "query",
                table: "images");

            migrationBuilder.DropColumn(
                name: "archived_at",
                schema: "query",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "status",
                schema: "query",
                table: "albums");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                schema: "query",
                table: "albums",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
