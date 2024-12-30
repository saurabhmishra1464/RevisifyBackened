using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace revisifyBackened.Migrations
{
    /// <inheritdoc />
    public partial class changeinmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeHash",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeHash",
                table: "Questions");
        }
    }
}
