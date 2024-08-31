using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Domain_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "archived_at",
                schema: "domain",
                table: "albums");

            migrationBuilder.RenameColumn(
                name: "accessibility",
                schema: "domain",
                table: "albums",
                newName: "access_level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "access_level",
                schema: "domain",
                table: "albums",
                newName: "accessibility");

            migrationBuilder.AddColumn<DateTime>(
                name: "archived_at",
                schema: "domain",
                table: "albums",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
