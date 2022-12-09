using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPGDOTNET.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationToTheDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "User");
        }
    }
}
