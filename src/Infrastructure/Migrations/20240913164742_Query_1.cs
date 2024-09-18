using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Query_1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "avatar_url", schema: "query", table: "users");

        migrationBuilder.DropColumn(name: "header_url", schema: "query", table: "users");

        migrationBuilder.AddColumn<DateTime>(
            name: "registered_at",
            schema: "query",
            table: "users",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "registered_at", schema: "query", table: "users");

        migrationBuilder.AddColumn<string>(
            name: "avatar_url",
            schema: "query",
            table: "users",
            type: "text",
            nullable: true
        );

        migrationBuilder.AddColumn<string>(
            name: "header_url",
            schema: "query",
            table: "users",
            type: "text",
            nullable: true
        );
    }
}
