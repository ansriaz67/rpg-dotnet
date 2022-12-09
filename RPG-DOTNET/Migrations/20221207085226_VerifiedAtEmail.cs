using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPGDOTNET.Migrations
{
    /// <inheritdoc />
    public partial class VerifiedAtEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "User",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "User");
        }
    }
}
