using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.DomainDb;

/// <inheritdoc />
public partial class Domain_3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "_password_salt",
            schema: "domain",
            table: "users",
            newName: "password_salt"
        );

        migrationBuilder.RenameColumn(
            name: "_password_hash",
            schema: "domain",
            table: "users",
            newName: "password_hash"
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "password_salt",
            schema: "domain",
            table: "users",
            newName: "_password_salt"
        );

        migrationBuilder.RenameColumn(
            name: "password_hash",
            schema: "domain",
            table: "users",
            newName: "_password_hash"
        );
    }
}
