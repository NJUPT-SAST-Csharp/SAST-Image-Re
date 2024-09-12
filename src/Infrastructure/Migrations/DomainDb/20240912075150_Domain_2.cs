using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.DomainDb
{
    /// <inheritdoc />
    public partial class Domain_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "_password_hash",
                schema: "domain",
                table: "users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "_password_salt",
                schema: "domain",
                table: "users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_password_hash",
                schema: "domain",
                table: "users");

            migrationBuilder.DropColumn(
                name: "_password_salt",
                schema: "domain",
                table: "users");
        }
    }
}
