using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    /// <inheritdoc />
    public partial class Query_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "archived_at",
                schema: "query",
                table: "albums");

            migrationBuilder.RenameColumn(
                name: "accessibility",
                schema: "query",
                table: "images",
                newName: "access_level");

            migrationBuilder.RenameColumn(
                name: "accessibility",
                schema: "query",
                table: "albums",
                newName: "access_level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "access_level",
                schema: "query",
                table: "images",
                newName: "accessibility");

            migrationBuilder.RenameColumn(
                name: "access_level",
                schema: "query",
                table: "albums",
                newName: "accessibility");

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                schema: "query",
                table: "albums",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
