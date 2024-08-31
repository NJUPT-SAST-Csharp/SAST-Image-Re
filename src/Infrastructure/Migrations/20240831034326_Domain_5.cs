using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Domain_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_archived",
                schema: "domain",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "domain",
                table: "albums");

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "domain",
                table: "images",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "domain",
                table: "images",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                schema: "domain",
                table: "albums",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "domain",
                table: "albums",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "domain",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "removed_at",
                schema: "domain",
                table: "images");

            migrationBuilder.DropColumn(
                name: "status",
                schema: "domain",
                table: "images");

            migrationBuilder.DropColumn(
                name: "archived_at",
                schema: "domain",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "removed_at",
                schema: "domain",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "status",
                schema: "domain",
                table: "albums");

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                schema: "domain",
                table: "albums",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "domain",
                table: "albums",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
