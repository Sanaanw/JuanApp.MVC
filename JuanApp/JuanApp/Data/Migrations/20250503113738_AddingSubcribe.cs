using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingSubcribe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubcribed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubcribed",
                table: "AspNetUsers");
        }
    }
}
